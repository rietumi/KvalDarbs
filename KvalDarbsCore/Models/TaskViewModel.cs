using LogicCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KvalDarbsCore.Models
{
    public class TaskViewModel : IValidatableObject
    {
        public TaskViewModel()
        {
        }

        public TaskViewModel(Task task)
        {
            this.TaskId = task.Id;
            this.Time = task.Time;
            this.Repetition = task.Repetition;
            this.Exercise = task.ExerciseId;
        }

        public int? TaskId { get; set; }

        [Display(Name = "Time", ResourceType = typeof(Text))]
        public string Time { get; set; }

        [Display(Name = "Repetition", ResourceType = typeof(Text))]
        public string Repetition { get; set; }

        [Display(Name = "Exercise", ResourceType = typeof(Text))]
        public int? Exercise { get; set; }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrEmpty(this.Time)
                    && string.IsNullOrEmpty(this.Repetition)
                    && !this.Exercise.HasValue;
            }
        }

        /// <summary>
        /// Validates class.
        /// </summary>
        /// <param name="validationContext">Describes the context in which a validation check is performed.</param>
        /// <returns>Errors.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!this.IsEmpty)
            {
                if (string.IsNullOrEmpty(this.Time) && string.IsNullOrEmpty(this.Repetition))
                    yield return new ValidationResult(string.Format(ErrorText.AtLeastOneOfTwoRequired, Text.Time, Text.Repetition));

                if (!this.Exercise.HasValue)
                    yield return new ValidationResult(string.Format(ErrorText.Required, Text.Exercise));
            }
        }
    }
}
