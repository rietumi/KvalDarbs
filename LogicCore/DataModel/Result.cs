﻿using LogicCore.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LogicCore
{
    public class Result : IEntity
    {
        public int? Id { get; set; }

        [RequiredLocalized]
        public string AthleteId { get; set; }

        public ApplicationUser Athlete { get; set; }

        [RequiredLocalized]
        public int? CompetitionId { get; set; }

        public Competition Competition { get; set; }

        [RequiredLocalized]
        [Range(0,99)]
        public int? Hours { get; set; }

        [RequiredLocalized]
        [Range(0, 59)]
        public int? Minutes { get; set; }

        [RequiredLocalized]
        [Range(0, 59)]
        public int? Seconds { get; set; }

        public List<Comment> Comments { get; set; }

        [NotMapped]
        public string Time
        {
            get
            {
                return $"{this.Hours}:{this.Minutes}:{this.Seconds}";
            }
        }
    }
}