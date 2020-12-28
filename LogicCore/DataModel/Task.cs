using System.Collections.Generic;

namespace LogicCore
{
    public class Task : IEntity
    {
        public Task()
        {
            this.Comments = new List<Comment>();
            this.Exercise = new Exercise();
        }

        public string Repetition { get; set; }

        public int? Id { get; set; }

        public string Time { get; set; }

        public List<Comment> Comments { get; set; }

        public int? TrainingId { get; set; }

        public int? ExerciseId { get; set; }

        public Exercise Exercise { get; set; }
    }
}