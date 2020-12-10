using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Result : IEntity, IValidation
    {
        public int Id { get; set; }

        public ApplicationUser Athlete { get; set; }

        public Competition Competition { get; set; }

        public DateTime? Time { get; set; }

        public List<Comment> Comments { get; set; }

        public bool Validate(ref List<Error> state)
        {
            throw new NotImplementedException();
        }
    }
}