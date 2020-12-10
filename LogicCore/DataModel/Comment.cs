using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public class Comment : IEntity
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public ApplicationUser Author { get; set; }
    }
}