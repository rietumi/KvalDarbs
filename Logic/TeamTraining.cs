using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Logic
{
    public class TeamTraining : IEntity
    {
        public List<Training> Trainings
        {
            get => default;
            set
            {
            }
        }

        public DateTime? Date { get; set; }

        public string Name { get; set; }

        public int Id
        {
            get => default;
            set
            {
            }
        }
    }
}