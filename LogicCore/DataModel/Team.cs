using System.Collections.Generic;

namespace LogicCore
{
    public class Team : IEntity
    {
        public ApplicationUser Coach { get; set; }

        public List<ApplicationUser> Members { get; set; }

        public List<TeamTraining> TeamTrainings { get; set; }

        public int Id { get; set; }

        public string Description { get; set; }

        public List<Goal> Goals { get; set; }
    }
}