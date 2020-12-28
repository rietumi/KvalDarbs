using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class TeamTraining : IEntity
    {
        public TeamTraining()
        {
            this.Trainings = new List<Training>();
        }

        public List<Training> Trainings { get; set; }

        public DateTime? Date { get; set; }

        public string Name { get; set; }

        public int? TeamId { get; set; }

        public int? Id { get; set; }
    }
}