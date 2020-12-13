using System;
using System.Collections.Generic;
using System.Text;

namespace LogicCore
{
    public class UserTeam
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int TeamId { get; set; }
        public Team Team { get; set; }
    }
}
