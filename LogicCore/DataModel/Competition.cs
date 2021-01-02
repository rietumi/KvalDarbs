using LogicCore.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public class Competition : IEntity
    {
        public int? Id { get; set; }

        [RequiredLocalized]
        [StringLengthLocalized(100)]
        [Display(Name = "Title", ResourceType = typeof(Text))]
        public string Name { get; set; }

        [RequiredLocalized]
        [Display(Name = "Date", ResourceType = typeof(Text))]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        [RequiredLocalized]
        [Display(Name = "Distance", ResourceType = typeof(Text))]
        public decimal Distance { get; set; }

        [RequiredLocalized]
        [Display(Name = "Type", ResourceType = typeof(Text))]
        public CompetitionType Type { get; set; }

        [RequiredLocalized]
        [StringLengthLocalized(400)]
        [Display(Name = "Location", ResourceType = typeof(Text))]
        public string Location { get; set; }

        public string AuthorId { get; set; }

        public ApplicationUser Author { get; set; }

        public List<Result> Results { get; set; } 
    }
}