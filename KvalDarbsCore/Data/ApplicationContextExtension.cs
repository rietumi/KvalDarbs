using LogicCore;
using LogicCore.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace KvalDarbsCore.Data
{
    public static class ApplicationContextExtension
    {
        /// <summary>
        /// Gets the current active user.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="httpContext">HTTP context.</param>
        /// <returns>Current active user.</returns>
        public static ApplicationUser GetActiveUser(this ApplicationDbContext context, HttpContext httpContext)
        {
            return context.Users.FirstOrDefault(m => m.Id == httpContext.User.GetLoggedInUserId<string>());
        }

        /// <summary>
        /// Gets the team by teams ID.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="id">Teams id.</param>
        /// <returns>Team.</returns>
        public static Team GetTeamById(this ApplicationDbContext context, int teamId)
        {
            var team = context.Teams.Include(m => m.Members).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == teamId);
            
            return team;
        }

        /// <summary>
        /// Checks whether the user is the coach of the team.
        /// </summary>
        /// <param name="context">DB context.</param>
        /// <param name="teamId">Teams ID.</param>
        /// <param name="user">Current user.</param>
        /// <returns>True if the user is the coach of the team, otherwise false.</returns>
        public static bool IsCoach(this ApplicationDbContext context, int teamId, ApplicationUser user)
        {
            if (user == null)
                return false;

            return context.Teams.FirstOrDefault(t => t.Id == teamId && t.Coach.Id == user.Id) != null;
        }
    }
}
