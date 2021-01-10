using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KvalDarbsCore.Data;
using LogicCore;

namespace KvalDarbsCore.Controllers
{
    public class CompetitionsController : AuthorizedController
    {
        public CompetitionsController(ApplicationDbContext context)
            : base (context)
        {
        }

        [HttpGet]
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSort"] = sortOrder == "Name" ? "name_desc" : "Name";
            ViewData["DateSort"] = string.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            ViewData["LocationSort"] = sortOrder == "Location" ? "location_desc" : "Location";
            ViewData["Sort"] = sortOrder;

            ViewData["SearchFilter"] = searchString;

            var competitions = _context.Competitions.Select(s => s);

            if (!string.IsNullOrEmpty(searchString))
            {
                competitions = competitions.Where(s => s.Name.Contains(searchString)
                                       || s.Location.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    competitions = competitions.OrderByDescending(s => s.Name);
                    break;
                case "Name":
                    competitions = competitions.OrderBy(s => s.Name);
                    break;
                case "date_desc":
                    competitions = competitions.OrderByDescending(s => s.Date);
                    break;
                case "Location":
                    competitions = competitions.OrderBy(s => s.Location);
                    break;
                case "location_desc":
                    competitions = competitions.OrderByDescending(s => s.Location);
                    break;
                default:
                    competitions = competitions.OrderBy(s => s.Date);
                    break;
            }

            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(competitions.ToList());
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = _context.Competitions.Include(m => m.Results).ThenInclude(m => m.Athlete)
                .FirstOrDefault(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(competition);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Id,Name,Date,Distance,Type,Location")] Competition competition)
        {
            var user = _context.GetActiveUser(this.HttpContext);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                competition.Author = user;
                _context.Add(competition);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var competition = _context.Competitions.FirstOrDefault(m => m.Id == id && m.AuthorId == user.Id);
            if (competition == null)
            {
                return NotFound();
            }
            return View(competition);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind("Id,Name,Date,Distance,Type,Location")] Competition competition)
        {
            if (id != competition.Id)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            if (!_context.Competitions.Any(m => m.Id == id && m.AuthorId == user.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    competition.AuthorId = user.Id;
                    _context.Update(competition);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(competition.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(competition);
        }

        [HttpGet]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations", Justification = "Returns different view from Details.")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var competition = _context.Competitions
                .FirstOrDefault(m => m.Id == id && m.AuthorId == user.Id);
            if (competition == null)
            {
                return NotFound();
            }

            return View(competition);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var competition = _context.Competitions.Include(m => m.Results).FirstOrDefault(m => m.Id == id && m.AuthorId == user.Id);

            if (competition == null)
                return NotFound();

            if (competition.Results.Any())
            {
                ViewData["ResultsExist"] = "Sacensībām ir saistīti rezultāti.";
                return View("Delete", competition);
            }

            _context.Competitions.Remove(competition);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult CreateResult(int? id)
        {
            ViewData["AthleteId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            ViewData["CompetitionId"] = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateResult([Bind("AthleteId,CompetitionId,Hours,Minutes,Seconds")] Result result)
        {
            var user = _context.GetActiveUser(this.HttpContext);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized();
            }

            if (!CompetitionExists(result.CompetitionId))
                return NotFound();

            if (ModelState.IsValid)
            {
                result.Athlete = user;
                result.AthleteId = user.Id;
                _context.Add(result);
                _context.SaveChanges();
                return RedirectToAction(nameof(Details), new { id = result.CompetitionId });
            }

            ViewData["AthleteId"] = user.Id;
            ViewData["CompetitionId"] = result.CompetitionId;
            return View(result);
        }

        [HttpGet]
        public ActionResult EditResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var result = _context.Results.FirstOrDefault(m => m.Id == id && m.AthleteId == user.Id);
            if (result == null)
            {
                return NotFound();
            }
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditResult(int? id, [Bind("Id,AthleteId,CompetitionId,Hours,Minutes,Seconds")] Result result)
        {
            if (id != result.Id)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            if (!_context.Results.Any(m => m.Id == id && m.AthleteId == user.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(result);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompetitionExists(result.CompetitionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Details), new { id = result.CompetitionId });
            }
            return View(result);
        }

        [HttpGet]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S4144:Methods should not have identical implementations", Justification = "Returns different view from Details.")]
        public ActionResult DeleteResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var result = _context.Results.Include(m => m.Athlete)
                .FirstOrDefault(m => m.Id == id && m.AthleteId == user.Id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }

        [HttpPost, ActionName("DeleteResult")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteResultConfirmed(int? id)
        {
            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var result = _context.Results.FirstOrDefault(m => m.Id == id && m.AthleteId == user.Id);

            if (result == null)
                return NotFound();

            _context.Results.Remove(result);
            _context.SaveChanges();
            return RedirectToAction(nameof(Details), new { id = result.CompetitionId});
        }

        [HttpGet]
        public ActionResult DetailsResult(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var result = _context.Results.Include(m => m.Athlete)
                .FirstOrDefault(m => m.Id == id);
            if (result == null)
            {
                return NotFound();
            }
            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(result);
        }

        private bool CompetitionExists(int? id)
        {
            return _context.Competitions.Any(e => e.Id == id);
        }
    }
}
