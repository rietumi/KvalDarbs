using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using KvalDarbsCore.Data;
using LogicCore;
using LogicCore.Util;

namespace KvalDarbsCore.Controllers
{
    public class TeamController : AuthorizedController
    {
        private readonly ApplicationDbContext _context;

        public TeamController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult Index()
        {
            Team team = null;

            var user = _context.Users.FirstOrDefault(m => m.Id == HttpContext.User.GetLoggedInUserId<string>());
            if (user != null)
                team = _context.Teams.FirstOrDefault(m => m.Coach.Id == user.Id);

            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create()
        {
            return View(new Team());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save()
        {
            return RedirectToAction("Index");
        }
    }
}
