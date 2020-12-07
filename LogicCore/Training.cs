using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Training : IEntity, IValidation
    {
        public int Id { get; set; }

        public List<Task> Tasks { get; set; }

        public User User { get; set; }
    }
}