using LogicCore.Util;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicCore
{
    public class Team : IEntity
    {
        [RequiredLocalized]
        [StringLength(100)]
        [Display(Name = "Title", ResourceType = typeof(Text))]
        public string Name { get; set; }

        public ApplicationUser Coach { get; set; }

        public List<UserTeam> Members { get; set; }

        public List<TeamTraining> TeamTrainings { get; set; }

        public int? Id { get; set; }

        [RequiredLocalized]
        [StringLength(4000)]
        [Display(Name = "Description", ResourceType = typeof(Text))]
        public string Description { get; set; }

        public List<Goal> Goals { get; set; }

        /// <summary>
        /// New member adding.
        /// </summary>
        [NotMapped]
        [Display(Name = "NewMember", ResourceType = typeof(Text))]
        public string NewMemberId { get; set; }

        /// <summary>
        /// New member adding.
        /// </summary>
        [NotMapped]
        public List<SelectListItem> PossibleMembers { get; set; }
    }
}