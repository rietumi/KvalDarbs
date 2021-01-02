using LogicCore.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace LogicCore
{
    public class Exercise : IEntity
    {
        public Exercise()
        {
            this.Examples = new List<Example>();
        }

        public List<Example> Examples { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        [RequiredLocalized]
        [StringLengthLocalized(4000)]
        [Display(Name = "Description", ResourceType = typeof(Text))]
        public string Description { get; set; }

        [RequiredLocalized]
        [StringLengthLocalized(100)]
        [Display(Name = "Title", ResourceType = typeof(Text))]
        public string Name { get; set; }

        public int? Id { get; set; }
    }
}