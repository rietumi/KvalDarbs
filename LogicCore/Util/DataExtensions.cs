using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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

        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Name ?? "Description Not Found";
        }
    }
}
