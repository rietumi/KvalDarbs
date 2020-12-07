using System;
using System.Collections.Generic;

namespace Logic
{
    public class Result : IEntity, IValidation
    {
        public int Id { get; set; }

        public User Athlete { get; set; }

        public Competition Competition { get; set; }

        public DateTime? Time { get; set; }

        public List<Comment> Comments { get; set; }
    }
}