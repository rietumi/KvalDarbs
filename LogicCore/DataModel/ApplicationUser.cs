using LogicCore.DataModel.Notifications;
using LogicCore.Util;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser() 
            : base()
        {
        }

        [EmailAddress]
        [StringLengthLocalized(50)]
        [RequiredLocalized]
        public override string Email { get; set; }

        [Phone]
        [StringLengthLocalized(20)]
        public override string PhoneNumber { get; set; }

        [StringLengthLocalized(50)]
        [RequiredLocalized]
        public string Name { get; set; }

        [StringLengthLocalized(50)]
        [RequiredLocalized]
        public string Surname { get; set; }

        public int Reminder { get; set; }

        public List<UserTeam> UserTeams { get; set; }

        public List<Notification> Notifications { get; set; }

        public string FullName
        {
            get
            {
                return string.Join(' ', this.Name, this.Surname);
            }
        }
    }
}