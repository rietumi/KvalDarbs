using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace LogicCore
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
            : base()
        {
        }

        public string Name { get; set; }

        public string Surname { get; set; }

        public int Reminder { get; set; }

        public List<UserTeam> UserTeams { get; set; }

        public string FullName
        {
            get
            {
                return string.Join(' ', this.Name, this.Surname);
            }
        }
    }
}