using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Result : IEntity
    {
        public int? Id { get; set; }

        public int? AthleteId { get; set; }

        public ApplicationUser Athlete { get; set; }

        public int? CompetitionId { get; set; }

        public DateTime? Time { get; set; }

        public List<Comment> Comments { get; set; }
    }
}