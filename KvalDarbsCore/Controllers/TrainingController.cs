using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KvalDarbsCore.Data;
using KvalDarbsCore.Models;
using LogicCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace KvalDarbsCore.Controllers
{
    public class TrainingController : AuthorizedController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<TrainingController> _logger;

        public TrainingController(ApplicationDbContext context, IMemoryCache memoryCache, ILogger<TrainingController> logger)
        {
            _context = context;
            _cache = memoryCache;
            _logger = logger;
        }

        public ActionResult Index()
        {
            var userId = _context.GetActiveUser(this.HttpContext)?.Id;
            var teamTrainings = _context.TeamTrainings.Include(m => m.Team).ThenInclude(m => m.Coach).Include(m => m.Trainings).ThenInclude(m => m.User).Where(m => m.Trainings.Count(m => m.UserId == userId) != 0).ToList();
            return View(teamTrainings);
        }

        [HttpGet]
        public ActionResult Add(int id)
        {
            var currentUser = _context.GetActiveUser(this.HttpContext);
            if (!_context.IsCoach(id, currentUser))
                return RedirectToAction("Open", "Team", new { id = id });

            var team = _context.Teams.Include(m => m.Members).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == id);

            if (team == null)
                return RedirectToAction("Index");

            var viewModel = new TeamTrainingViewModel(id);
            viewModel.Trainings = new List<TrainingViewModel>() {
                new TrainingViewModel() {
                    Tasks = new List<TaskViewModel>()
                    {
                        new TaskViewModel()
                    }
                }
            };

            // Can't assign training to coach.
            viewModel.Members = team.Members.Where(m => m.UserId != currentUser.Id).Select(m => new KeyValuePair<string, string>(m.UserId, m.User.FullName)).ToList();
            viewModel.Exercises = _context.Exercises.Select(m => new KeyValuePair<int?, string>(m.Id.Value, m.Name)).ToList();
            viewModel.Exercises.Insert(0, new KeyValuePair<int?, string>(null, string.Empty));
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(TeamTrainingViewModel teamTraining)
        {
            if (!_context.IsCoach(teamTraining.Team, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", "Team", new { id = teamTraining.Team });

            if (!this.ModelState.IsValid)
                return View("Add", teamTraining);

            using (var transaction = _context.Database.BeginTransaction())
            {
                var training = _context.TeamTrainings.Include(m => m.Trainings).ThenInclude(m => m.Tasks).ThenInclude(m => m.Exercise).FirstOrDefault(m => m.Id == teamTraining.TeamTrainingId && m.TeamId == teamTraining.Team);

                try
                {
                    if (training != null)
                    {
                        _context.TeamTrainings.Remove(training);
                        _context.SaveChanges();
                    }

                    var result = Mapper.MapTeamTraining(teamTraining, _context);
                    
                    var team = _context.Teams.Include(m => m.TeamTrainings).Include(m => m.Members).FirstOrDefault(m => m.Id == teamTraining.Team);

                    if (team != null)
                    {
                        team.TeamTrainings.Add(result);
                        _context.Teams.Update(team);
                        _context.SaveChanges();
                        new TeamController(_context, _cache).RemoveTeamCacheForUsers(team.Members.Select(m => m.UserId));
                    }
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    transaction.Rollback();
                    throw;
                }
            }

            return RedirectToAction("Open", "Team", new { id = teamTraining.Team });
        }

        [HttpGet]
        public ActionResult Open(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var teamTraining = _context.TeamTrainings.Include(m => m.Trainings).ThenInclude(m => m.Tasks).ThenInclude(m => m.Exercise).Include(m => m.Trainings).ThenInclude(m => m.User).AsNoTracking().FirstOrDefault(m => m.Id == id);
            
            if (teamTraining.Trainings == null)
                return RedirectToAction("Open", "Team", new { id = teamTraining.TeamId });

            var training = teamTraining.Trainings.FirstOrDefault(m => m.UserId == _context.GetActiveUser(this.HttpContext)?.Id);

            if (training == null)
            {
                if (!_context.IsCoach(teamTraining.TeamId, _context.GetActiveUser(this.HttpContext)))
                    return RedirectToAction("Open", "Team", new { id = teamTraining.TeamId });
                return View(teamTraining);
            }
            else
            {
                teamTraining.Trainings = new List<Training>() { training };
                return View(teamTraining);
            }
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (!id.HasValue)
                return NotFound();

            var result = new TeamTrainingViewModel();

            try
            {
                result = new TeamTrainingViewModel(_context, id.Value);
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
            }

            var currentUser = _context.GetActiveUser(this.HttpContext);
            if (!_context.IsCoach(result.Team, currentUser))
                return RedirectToAction("Open", "Team", new { id = result.Team });

            var team = _context.Teams.Include(m => m.Members).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == result.Team);

            if (team == null)
                return RedirectToAction("Index");

            result.Members = team.Members.Where(m => m.UserId != currentUser.Id).Select(m => new KeyValuePair<string, string>(m.UserId, m.User.FullName)).ToList();
            result.Exercises = _context.Exercises.Select(m => new KeyValuePair<int?, string>(m.Id.Value, m.Name)).ToList();
            result.Exercises.Insert(0, new KeyValuePair<int?, string>(null, string.Empty));

            return View("Add", result);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var tt = _context.TeamTrainings.FirstOrDefault(m => m.Id == id);

            if (tt == null || !_context.IsCoach(tt.TeamId, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", "Team", new { id = tt.TeamId });

            var training = _context.TeamTrainings.Include(m => m.Trainings).ThenInclude(m => m.Tasks).ThenInclude(m => m.Exercise).FirstOrDefault(m => m.Id == tt.Id);
            _context.TeamTrainings.Remove(training);
            _context.SaveChanges();

            return RedirectToAction("Open", "Team", new { id = tt.TeamId });
        }

        /// <summary>
        /// Add exercise to exercise list(VM-06).
        /// </summary>
        /// <param name="exercise">Exercise.</param>
        /// <returns>Exercise for dropdown or error list.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult AddExercise([Bind("Description,Name,Id")] Exercise exercise)
        {
            if (!ModelState.IsValid)
            {
                return new JsonResult(new KeyValuePair<int, string>(0, string.Join(";\n", ModelState.Values
                                        .SelectMany(x => x.Errors)
                                        .Select(x => x.ErrorMessage))));
            }

            _context.Exercises.Add(exercise);
            _context.SaveChanges();

            return new JsonResult(new KeyValuePair<int, string>(exercise.Id.Value, exercise.Name));
        }
    }
}
