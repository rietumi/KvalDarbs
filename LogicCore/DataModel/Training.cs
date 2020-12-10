using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Training : IEntity, IValidation
    {
        public int Id { get; set; }

        public List<Task> Tasks { get; set; }

        public ApplicationUser User { get; set; }

        public bool Validate(ref List<Error> state)
        {
            throw new NotImplementedException();
        }
    }
}