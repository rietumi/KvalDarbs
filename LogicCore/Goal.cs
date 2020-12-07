using System;

namespace LogicCore
{
    public class Goal : IEntity, IValidation
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public DateTime? Deadline { get; set; }

        public int Status
        {
            get => default;
            set
            {
            }
        }

        public PriorityType Priority
        {
            get => default;
            set
            {
            }
        }
    }
}