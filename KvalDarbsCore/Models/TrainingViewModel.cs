using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using LogicCore;

namespace KvalDarbsCore.Models
{
    public class TrainingViewModel : IValidatableObject
    {
        public TrainingViewModel()
        {
        }

        public TrainingViewModel(Training training)
        {
            this.TrainingId = training.Id;
            this.User = training.UserId;
            this.Tasks = new List<TaskViewModel>();
            foreach (var task in training.Tasks)
            {
                this.Tasks.Add(new TaskViewModel(task));
            }
        }

        public int? TrainingId { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Text))]
        public string User { get; set; }

        public List<TaskViewModel> Tasks { get; set; }

        /// <summary>
        /// Validates class.
        /// </summary>
        /// <param name="validationContext">Describes the context in which a validation check is performed.</param>
        /// <returns>Errors.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.User))
                yield return new ValidationResult(string.Format(ErrorText.Required, Text.Name));
        }
    }
}
