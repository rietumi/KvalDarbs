using KvalDarbsCore.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KvalDarbsCore.Models
{
    public class TeamTrainingViewModel
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
            // TODO: Atlasīt komandas treniņus.
            var teamTraining = context.TeamTrainings.Include(m => m.Trainings).ThenInclude(m => m.Tasks).ThenInclude(m => m.Task).ThenInclude(m => m.Exercise).ThenInclude(m => m.Examples).Include(m => m.Trainings).ThenInclude(m => m.User).FirstOrDefault(m => m.Id == trainingId);
        }

        public int Team { get; set; }
        public string Name { get; set; }
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
        public List<KeyValuePair<string, string>> Exercise { get; set; }
    }
}
