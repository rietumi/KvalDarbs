using LogicCore.Util;
using System;
using System.Collections.Generic;

namespace LogicCore
{
    public class Training : IEntity
    {
        public Training()
        {
            this.Tasks = new List<Task>();
            this.User = new ApplicationUser();
        }

        public int? Id { get; set; }

        [RequiredLocalized]
        public int? TeamTrainingId { get; set; }

        [RequiredLocalized]
        public string UserId { get; set; }

        public List<Task> Tasks { get; set; }

        public ApplicationUser User { get; set; }
    }
}