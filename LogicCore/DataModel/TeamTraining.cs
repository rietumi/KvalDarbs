using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogicCore
{
    public class TeamTraining : IEntity
    {
        public List<Training> Trainings { get; set; }

        public DateTime Date { get; set; }

        public string Name { get; set; }

        public int Id { get; set; }
    }
}