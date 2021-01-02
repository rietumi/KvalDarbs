using LogicCore.Util;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LogicCore
{
    public class Task : IEntity
    {
        public Task()
        {
            this.Comments = new List<Comment>();
            this.Exercise = new Exercise();
        }

        [StringLengthLocalized(100)]
        public string Repetition { get; set; }

        public int? Id { get; set; }

        [StringLengthLocalized(100)]
        public string Time { get; set; }

        public List<Comment> Comments { get; set; }

        [RequiredLocalized]
        public int? TrainingId { get; set; }

        [RequiredLocalized]
        public int? ExerciseId { get; set; }

        public Exercise Exercise { get; set; }
    }
}