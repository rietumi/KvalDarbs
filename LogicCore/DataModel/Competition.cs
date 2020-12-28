using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Competition : IEntity
    {
        public int? Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public decimal Distance { get; set; }

        public CompetitionType Type { get; set; }

        public string Location { get; set; }

        public List<Result> Results { get; set; } 
    }
}