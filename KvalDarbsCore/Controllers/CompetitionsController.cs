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

        // GET: Competitions
        public ActionResult Index(string sortOrder, string searchString)
        {
            ViewData["NameSort"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : string.Empty;
            ViewData["DateSort"] = sortOrder == "Date" ? "date_desc" : "Date";
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
                case "Date":
                    competitions = competitions.OrderBy(s => s.Date);
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
                    competitions = competitions.OrderBy(s => s.Name);
                    break;
            }

            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(competitions.ToList());
        }

        // GET: Competitions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var competition = _context.Competitions
                .FirstOrDefault(m => m.Id == id);
            if (competition == null)
            {
                return NotFound();
            }

            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(competition);
        }

        // GET: Competitions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Competitions/Edit/5
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

        // POST: Competitions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

            if (_context.Competitions.Any(m => m.Id == id && m.AuthorId == user.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
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

        // GET: Competitions/Delete/5
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

        // POST: Competitions/Delete/5
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

            _context.Competitions.Remove(competition);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public ActionResult CreateResult(int? id)
        {
            ViewData["AthleteId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            ViewData["CompetitionId"] = id;
            return View();
        }

        // POST: Competitions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
                _context.Add(result);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewData["AthleteId"] = user.Id;
            ViewData["CompetitionId"] = result.CompetitionId;
            return View(result);
        }

        private bool CompetitionExists(int? id)
        {
            return _context.Competitions.Any(e => e.Id == id);
        }
    }
}
