using KvalDarbsCore.Data;
using LogicCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Linq;

namespace KvalDarbsCore.Controllers
{
    [Authorize]
    public class AuthorizedController : Controller
    {
        public readonly ApplicationDbContext _context;

        public AuthorizedController(ApplicationDbContext context)
        {
            this._context = context;
        }

        [HttpPost]
        public JsonResult Notifications()
        {
            var user = _context.GetActiveUser(this.HttpContext);
            if (user == null)
                return Json("Failed");

            var notifications = _context.Notifications.Where(n => n.UserId == user.Id && n.Actual).OrderByDescending(m => m.CreationDate).Select(m => new { m.Id, m.Message }).ToArray();

            return Json(notifications);
        }

        [HttpPost]
        public JsonResult ApproveNotification(int? id)
        {
            var user = _context.GetActiveUser(this.HttpContext);
            var notification = _context.Notifications.Find(id);
            if (notification == null || user == null || notification.UserId != user.Id)
                return Json("Failed");

            // If connection between user and team already exists then fail.
            if (_context.UserTeams.FirstOrDefault(m => m.TeamId == notification.TeamId && m.UserId == notification.UserId) != null)
                return Json("Failed");

            try
            {
                notification.Actual = false;
                _context.Update(notification);
                _context.UserTeams.Add(new UserTeam { TeamId = notification.TeamId.Value, UserId = notification.UserId });
                _context.SaveChanges();
            }
            catch
            {
                return Json("Failed");
            }

            return Json("Success");
        }

        [HttpPost]
        public JsonResult DismissNotification(int? id)
        {
            var user = _context.GetActiveUser(this.HttpContext);
            var notification = _context.Notifications.Find(id);
            if (notification == null || user == null || notification.UserId != user.Id)
                return Json("Failed");

            try
            {
                notification.Actual = false;
                _context.Update(notification);
                _context.SaveChanges();
            }
            catch
            {
                return Json("Failed");
            }

            return Json("Success");
        }
    }
}
