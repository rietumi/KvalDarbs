using LogicCore.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public class TeamTraining : IEntity
    {
        public TeamTraining()
        {
            this.Trainings = new List<Training>();
        }

        public List<Training> Trainings { get; set; }

        [RequiredLocalized]
        public DateTime? Date { get; set; }

        [RequiredLocalized]
        [StringLengthLocalized(100)]
        public string Name { get; set; }

        [RequiredLocalized]
        public int? TeamId { get; set; }

        public Team Team { get; set; }

        public int? Id { get; set; }
    }
}