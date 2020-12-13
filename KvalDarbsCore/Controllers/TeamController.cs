using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KvalDarbsCore.Data;
using LogicCore;
using LogicCore.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.EntityFrameworkCore;

namespace KvalDarbsCore.Controllers
{
    public class TeamController : AuthorizedController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private const string _teamsKey = "Teams_{0}";

        public TeamController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        [HttpGet]
        public ActionResult Index()
        {
            List<Team> teams = new List<Team>();

            var user = _context.Users.FirstOrDefault(m => m.Id == HttpContext.User.GetLoggedInUserId<string>());

            if (user != null)
            {
                var key = string.Format(_teamsKey, user.Id);

                if (!_cache.TryGetValue(key, out teams))
                {
                    teams = _context.Teams.Include(m => m.Members).ThenInclude(m => m.User).Where(t => t.Members.FirstOrDefault(m => m.UserId == user.Id) != null).ToList();
                    _cache.Set(key, teams, TimeSpan.FromMinutes(5));
                }
            }

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

            var user = _context.GetActiveUser(this.HttpContext);
            var userTeam = new UserTeam { TeamId = team.Id, UserId = user.Id, User = user, Team = team };

            team.Coach = user;

            _context.UserTeams.Add(userTeam);
            _context.Teams.Add(team);
            _context.SaveChanges();
            this.RemoveTeamCacheForUsers(_context.Teams.Include(m => m.Members).FirstOrDefault(m => m.Id == team.Id).Members.Select(m => m.UserId));

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Open(int id)
        {
            var team = _context.GetTeamById(id);
            var user = _context.GetActiveUser(this.HttpContext);

            if (!team.BelongsToTeam(user))
                return RedirectToAction("Index");

            // Selects all users that aren't part of the team.
            team.PossibleMembers = new List<SelectListItem>() { new SelectListItem(string.Empty, string.Empty) };
            team.PossibleMembers.AddRange(_context.Users.Where(u => _context.UserTeams.FirstOrDefault(ut => ut.TeamId == team.Id && ut.UserId == u.Id) == null).Select(m => new SelectListItem(m.FullName, m.Id)).AsEnumerable().OrderBy(s => s.Text));

            var memberError = TempData["MemberError"] as string;
            if (!string.IsNullOrEmpty(memberError))
                ModelState.AddModelError("", memberError);

            ViewData["IsCoach"] = _context.IsCoach(team.Id, _context.GetActiveUser(this.HttpContext));

            return View(team);
        }

        [HttpPost]
        public ActionResult AddMember(Team team)
        {
            if (!_context.IsCoach(team.Id, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", new { id = team.Id });

            var newMember = _context.Users.FirstOrDefault(m => m.Id == team.NewMemberId);

            if (newMember != null)
            {
                _context.UserTeams.Add(new UserTeam { TeamId = team.Id, User = newMember, UserId = newMember.Id });
                _context.SaveChanges();
                this.RemoveTeamCacheForUsers(_context.Teams.Include(m => m.Members).FirstOrDefault(m => m.Id == team.Id).Members.Select(m => m.UserId));
            }
            else
            {
                TempData["MemberError"] = "Neizdevās pievienot lietotāju.";
            }

            return RedirectToAction("Open", new { id = team.Id });
        }

        [HttpGet]
        public ActionResult DeleteMember(string userId, int teamId)
        {
            if (!_context.IsCoach(teamId, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", new { id = teamId });

            var deletable = _context.UserTeams.FirstOrDefault(m => m.TeamId == teamId && m.UserId == userId);

            if (deletable != null)
            {
                _context.UserTeams.Remove(deletable);
                _context.SaveChanges();
                this.RemoveTeamCacheForUsers(_context.Teams.Include(m => m.Members).FirstOrDefault(m => m.Id == teamId).Members.Select(m => m.UserId));
            }

            return RedirectToAction("Open", new { id = teamId });
        }

        [HttpGet]
        public ActionResult DeleteTeam(int teamId)
        {
            if (!_context.IsCoach(teamId, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", new { id = teamId });

            var deletable = _context.Teams.FirstOrDefault(m => m.Id == teamId);

            if (deletable != null)
            {
                _context.Teams.Remove(deletable);
                _context.SaveChanges();
                this.RemoveTeamCacheForUsers(deletable.Members.Select(m => m.UserId));
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Maintains that users have updated team statuses.
        /// </summary>
        /// <param name="userIds">User ids.</param>
        private void RemoveTeamCacheForUsers(IEnumerable<string> userIds)
        {
            foreach (var userId in userIds)
                _cache.Remove(string.Format(_teamsKey, userId));
        }
    }
}
