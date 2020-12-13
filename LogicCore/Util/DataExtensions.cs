using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicCore.Util
{
    public static class DataExtensions
    {
        public static bool BelongsToTeam(this Team team, ApplicationUser user)
        {
            if (user == null)
                return false;

            return team.Members.FirstOrDefault(m => m.UserId == user.Id) != null;
        }
    }
}
