using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KvalDarbsCore.Data;
using LogicCore;

namespace KvalDarbsCore.Controllers
{
    public class ExercisesController : AuthorizedController
    {
        public ExercisesController(ApplicationDbContext context)
            : base (context)
        {
        }

        /// <summary>
        /// Opens exercise list (VM-05).
        /// </summary>
        /// <returns>View with list of exercises.</returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(_context.Exercises.ToList());
        }

        /// <summary>
        /// Opens exercise list (VM-05).
        /// </summary>
        /// <returns>View with list of exercises.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(string searchString)
        {
            var result = _context.Exercises.ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper();
                result = result.Where(m => m.Name.ToUpper().Contains(searchString)
                                    || m.Description.ToUpper().Contains(searchString)).ToList();
            }

            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(result);
        }

        /// <summary>
        /// Opens exercise detailed view(VM-03).
        /// </summary>
        /// <param name="id">Exercise's ID.</param>
        /// <returns>Opened exercise.</returns>
        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exercise = _context.Exercises
                .FirstOrDefault(m => m.Id == id);
            if (exercise == null)
            {
                return NotFound();
            }

            ViewData["ActiveUserId"] = _context.GetActiveUser(this.HttpContext)?.Id;
            return View(exercise);
        }

        /// <summary>
        /// Opens exercise for creation(VM-01).
        /// </summary>
        /// <returns>View for exercise creation.</returns>
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Saves newly created exercise(VM-01).
        /// </summary>
        /// <param name="exercise">Exercise.</param>
        /// <returns>Redirect to Index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("Description,Name,Id")] Exercise exercise)
        {
            var user = _context.GetActiveUser(this.HttpContext);

            if (user == null || string.IsNullOrEmpty(user.Id))
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                exercise.Author = user;
                _context.Add(exercise);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(exercise);
        }

        /// <summary>
        /// Opens exercise for editing(VM-04).
        /// </summary>
        /// <param name="id">Exercise's ID.</param>
        /// <returns>Editing view with exercise to be edited.</returns>
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

            var exercise = _context.Exercises.FirstOrDefault(m => m.Id == id && m.AuthorId == user.Id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        /// <summary>
        /// Saves edited exercise(VM-04).
        /// </summary>
        /// <param name="id">Exercise's ID.</param>
        /// <param name="exercise">Exercise.</param>
        /// <returns>After success - redirects to index.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, [Bind("Description,Name,Id")] Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            if (!_context.Exercises.Any(m => m.Id == id && m.AuthorId == user.Id))
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    exercise.AuthorId = user.Id;
                    _context.Update(exercise);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseExists(exercise.Id))
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
            return View(exercise);
        }

        /// <summary>
        /// Opens exercise for deletion(VM-02).
        /// </summary>
        /// <param name="id">Exercise's ID.</param>
        /// <returns>Delete view with exercise to be deleted.</returns>
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

            var exercise = _context.Exercises
                .FirstOrDefault(m => m.Id == id && m.AuthorId == user.Id);
            if (exercise == null)
            {
                return NotFound();
            }

            return View(exercise);
        }

        /// <summary>
        /// Exercise's deletion(VM-02).
        /// </summary>
        /// <param name="id">Exercise's ID.</param>
        /// <returns>Redirect to Index.</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null || string.IsNullOrEmpty(user.Id))
                return Unauthorized();

            var exercise = _context.Exercises.FirstOrDefault(m => m.Id == id && m.AuthorId == user.Id);

            if (exercise == null)
                return NotFound();

            _context.Exercises.Remove(exercise);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseExists(int? id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }
    }
}
