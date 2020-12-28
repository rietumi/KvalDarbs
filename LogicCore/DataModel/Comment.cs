using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public class Comment : IEntity
    {
        public int? Id { get; set; }

        public string Text { get; set; }

        public int? TaskId { get; set; }

        public int? ResultId { get; set; }

        public ApplicationUser Author { get; set; }
    }
}