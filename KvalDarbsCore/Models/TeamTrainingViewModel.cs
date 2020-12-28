using KvalDarbsCore.Data;
using LogicCore;
using LogicCore.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace KvalDarbsCore.Models
{
    public class TeamTrainingViewModel : IValidatableObject
    {
        public TeamTrainingViewModel()
        {
        }

        public TeamTrainingViewModel(int teamId)
        {
            this.Team = teamId;
        }

        public TeamTrainingViewModel(ApplicationDbContext context, int trainingId)
        {
            var teamTraining = context.TeamTrainings.Include(m => m.Trainings).ThenInclude(m => m.Tasks).ThenInclude(m => m.Exercise).Include(m => m.Trainings).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == trainingId);
            this.Team = teamTraining.TeamId.Value;
            this.Name = teamTraining.Name;
            this.Date = teamTraining.Date;
            this.TeamTrainingId = trainingId;
            this.Trainings = new List<TrainingViewModel>();
            foreach (var training in teamTraining.Trainings)
            {
                this.Trainings.Add(new TrainingViewModel(training));
            }
        }

        public int Team { get; set; }

        public int? TeamTrainingId { get; set; }

        [RequiredLocalized]
        [Display(Name = "Title", ResourceType = typeof(Text))]
        public string Name { get; set; }

        [RequiredLocalized]
        [Display(Name = "Date", ResourceType = typeof(Text))]
        public DateTime? Date { get; set; }

        public List<TrainingViewModel> Trainings { get; set; }

        /// <summary>
        /// Key - user's id,
        /// Value - user's fullname.
        /// </summary>
        public List<KeyValuePair<string, string>> Members { get; set; }

        /// <summary>
        /// Key - exercise id,
        /// Value - exercise fullname.
        /// </summary>
        public List<KeyValuePair<int?, string>> Exercises { get; set; }

        /// <summary>
        /// Validates class.
        /// </summary>
        /// <param name="validationContext">Describes the context in which a validation check is performed.</param>
        /// <returns>Errors.</returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errors = new List<ValidationResult>();
            var list = new List<string>();
            if (this.Trainings != null && this.Trainings.Any())
            {
                this.Trainings.ForEach(m => {
                    if (m.Tasks == null || !m.Tasks.Any())
                        errors.Add(new ValidationResult(string.Format(ErrorText.AtLeastOneTask, this.Members.FirstOrDefault(u => u.Key == m.User).Value)));

                    if (!this.Members.Exists(u => u.Key == m.User))
                        errors.Add(new ValidationResult(string.Format("Lietotājs {0} nav komandas dalībnieks.", this.Members.FirstOrDefault(u => u.Key == m.User).Value)));

                    if (list.Contains(m.User))
                        errors.Add(new ValidationResult(string.Format("Lietotājam {0} ir izveidoti vairāki treniņi.", this.Members.FirstOrDefault(u => u.Key == m.User).Value)));
                    else
                        list.Add(m.User);

                    m.Tasks.ForEach(t => {
                        if (this.Exercises != null && !t.IsEmpty && !this.Exercises.Exists(e => t.Exercise.HasValue && e.Key == t.Exercise))
                            errors.Add(new ValidationResult(string.Format(ErrorText.DoesntExist, Text.Exercise)));
                    });
                });
            }
            else
            {
                errors.Add(new ValidationResult(ErrorText.AtLeastOneTraining));
            }

            foreach (var error in errors)
                yield return error;
        }
    }
}
