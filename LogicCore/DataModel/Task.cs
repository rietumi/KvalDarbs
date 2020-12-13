using System.Collections.Generic;

namespace LogicCore
{
    public class Task : IEntity, IValidation
    {

        public string Repetition { get; set; }

        public int Id { get; set; }

        public string Time { get; set; }

        public List<Comment> Comments { get; set; }

        public List<TrainingTask> Trainings { get; set; }

        public Exercise Exercise { get; set; }

        public bool Validate(ref List<Error> state)
        {
            throw new System.NotImplementedException();
        }
    }
}