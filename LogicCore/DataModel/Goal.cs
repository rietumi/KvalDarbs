using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Goal : IEntity
    {
        public int? Id { get; set; }

        public int? TeamId { get; set; }

        public string Description { get; set; }

        public DateTime? Deadline { get; set; }

        public int Status { get; set; }

        public PriorityType Priority { get; set; }
    }
}