using System;

namespace Logic
{
    public class Competition : IEntity, IValidation
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? Date { get; set; }

        public decimal Distance { get; set; }

        public CompetitionType Type { get; set; }

        public string Location { get; set; }
    }
}