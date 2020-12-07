using System.Collections.Generic;

namespace LogicCore
{
    public class Task : IEntity, IValidation
    {

        public string Repetiton { get; set; }

        public int Id { get; set; }

        public string Time { get; set; }

        public List<Comment> Comments { get; set; }
    }
}