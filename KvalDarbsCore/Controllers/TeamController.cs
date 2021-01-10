using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using KvalDarbsCore.Data;
using LogicCore;
using LogicCore.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;
using LogicCore.DataModel.Notifications;

namespace KvalDarbsCore.Controllers
{
    public class TeamController : AuthorizedController
    {
        public TeamController(ApplicationDbContext context)
            : base (context)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var user = _context.GetActiveUser(this.HttpContext);

            if (user == null)
            {
                return Unauthorized();
            }

            List<Team> teams = _context.Teams.Include(m => m.Members).ThenInclude(m => m.User).Where(t => t.Members.FirstOrDefault(m => m.UserId == user.Id) != null).ToList();

            return View(teams);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Team team)
        {
            if (!this.ModelState.IsValid)
                return View(team);

            if (team.Id.HasValue)
                return RedirectToAction("Index");

            _context.Teams.Add(team);
            _context.SaveChanges();

            var user = _context.GetActiveUser(this.HttpContext);
            var userTeam = new UserTeam { TeamId = team.Id.Value, UserId = user.Id, User = user, Team = team };

            team.Coach = user;

            _context.UserTeams.Add(userTeam);
            _context.Teams.Update(team);
            _context.SaveChanges();

            return RedirectToAction("Open", new { id = team.Id });
        }

        [HttpGet]
        public ActionResult Open(int? id, int? ResultFilter)
        {
            if (!id.HasValue)
                return NotFound();

            var team = _context.GetTeamById(id.Value);
            var user = _context.GetActiveUser(this.HttpContext);

            if (team == null || !team.BelongsToTeam(user))
                return RedirectToAction("Index");

            // Selects all users that aren't part of the team.
            team.PossibleMembers = new List<SelectListItem>() { new SelectListItem(string.Empty, string.Empty) };
            team.PossibleMembers.AddRange(_context.Users.Where(u => _context.UserTeams.FirstOrDefault(ut => ut.TeamId == team.Id && ut.UserId == u.Id) == null).Select(m => new SelectListItem(m.FullName, m.Id)).AsEnumerable().OrderBy(s => s.Text));
            team.Results = new List<Result>();
            team.Results.AddRange(_context.Results.Include(m => m.Athlete).Include(m => m.Competition).Where(r => _context.UserTeams.FirstOrDefault(m => m.TeamId == team.Id && r.AthleteId == m.UserId) != null));
            
            if (ResultFilter.HasValue)
            {
                team.Results = team.Results.Where(m => (int)m.Competition.Type == ResultFilter.Value).ToList();
            }

            team.Results = team.Results.OrderBy(r => r.Time).ToList();

            var memberError = TempData["MemberError"] as string;
            if (!string.IsNullOrEmpty(memberError))
                ModelState.AddModelError("", memberError);

            ViewData["IsCoach"] = _context.IsCoach(team.Id, _context.GetActiveUser(this.HttpContext));

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddMember(Team team)
        {
            if (!_context.IsCoach(team.Id, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", new { id = team.Id });

            var newMember = _context.Users.FirstOrDefault(m => m.Id == team.NewMemberId);

            if (newMember == null)
            {
                TempData["MemberError"] = "Neizdevās pievienot lietotāju.";
            }
            else if (_context.Notifications.FirstOrDefault(n => n.TeamId == team.Id && n.UserId == newMember.Id && n.Actual) != null)
            {
                TempData["MemberError"] = "Lietotājam jau ir nosūtīts uzaicinājums.";
            }
            else
            {
                var notification = new Notification(team.Id, newMember.Id, $"Komanda \"{team.Name}\" vēlas, lai Jūs pievienojaties");
                _context.Notifications.Add(notification);
                _context.SaveChanges();
                TempData["MemberError"] = "Lietotājam veiksmīgi nosūtīts uzaicinājums.";
            }

            return RedirectToAction("Open", new { id = team.Id });
        }

        [HttpGet]
        public ActionResult DeleteTeam(int teamId)
        {
            if (!_context.IsCoach(teamId, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", new { id = teamId });

            var deletable = _context.Teams.Include(m => m.Notifications).Include(m => m.Members).Include(m => m.TeamTrainings).ThenInclude(m => m.Trainings).ThenInclude(m => m.Tasks).FirstOrDefault(m => m.Id == teamId);

            if (deletable != null)
            {
                _context.Teams.Remove(deletable);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteMember(string userId, int teamId)
        {
            var currentUser = _context.GetActiveUser(this.HttpContext);
            var isCoach = _context.IsCoach(teamId, currentUser);

            // Coach can't delete himself from the team.
            if (!isCoach || (isCoach && currentUser.Id == userId))
                return RedirectToAction("Open", new { id = teamId });

            var deletable = _context.UserTeams.FirstOrDefault(m => m.TeamId == teamId && m.UserId == userId);

            if (deletable != null)
            {
                _context.UserTeams.Remove(deletable);
                _context.SaveChanges();
            }

            return RedirectToAction("Open", new { id = teamId });
        }

        [HttpGet]
        public ActionResult AssignCoach(string userId, int teamId)
        {
            var currentUser = _context.GetActiveUser(this.HttpContext);
            var isCoach = _context.IsCoach(teamId, currentUser);

            // Coach can't assign himself as new coach.
            if (!isCoach || (isCoach && currentUser.Id == userId))
                return RedirectToAction("Open", new { id = teamId });

            var team = _context.Teams.Find(teamId);
            var user = _context.Users.Find(userId);

            if (team != null && user != null)
            {
                team.Coach = user;
                _context.Update(team);
                _context.SaveChanges();
            }
            else
            {
                return NotFound();
            }

            return RedirectToAction("Open", new { id = teamId });
        }
    }
}
