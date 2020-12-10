using Microsoft.AspNetCore.Identity;

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

        public string FullName
        {
            get
            {
                return string.Join(' ', this.Name, this.Surname);
            }
        }
    }
}