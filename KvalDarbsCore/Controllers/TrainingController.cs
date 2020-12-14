using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KvalDarbsCore.Data;
using KvalDarbsCore.Models;
using LogicCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace KvalDarbsCore.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public TrainingController(ApplicationDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _cache = memoryCache;
        }

        public ActionResult Index()
        {
            // Atgriezt visus treniņus, kas ir lietotājam
            return View();
        }

        [HttpGet]
        public ActionResult Add(int teamId)
        {
            if (!_context.IsCoach(teamId, _context.GetActiveUser(this.HttpContext)))
                return RedirectToAction("Open", "Team", new { id = teamId });

            var team = _context.Teams.Include(m => m.Members).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == teamId);

            if (team == null)
                return RedirectToAction("Index");

            var viewModel = new TeamTrainingViewModel(teamId);
            viewModel.Trainings = new List<TrainingViewModel>() {
                new TrainingViewModel() {
                    Tasks = new List<TaskViewModel>()
                    {
                        new TaskViewModel()
                    }
                } 
            };

            viewModel.Members = team.Members.Select(m => new KeyValuePair<string, string>(m.UserId, m.User.FullName)).ToList();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(TeamTrainingViewModel training)
        {
            return new JsonResult(JsonConvert.SerializeObject(training));
        }
    }
}
