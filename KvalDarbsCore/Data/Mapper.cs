using KvalDarbsCore.Models;
using LogicCore;
using System.Collections.Generic;
using System.Linq;

namespace KvalDarbsCore.Data
{
    public static class Mapper
    {
        public static TeamTraining MapTeamTraining(TeamTrainingViewModel teamTraining, ApplicationDbContext context)
        {
            var result = new TeamTraining()
            {
                TeamId = teamTraining.Team,
                Name = teamTraining.Name,
                Date = teamTraining.Date
            };

            foreach (var training in teamTraining.Trainings)
            {
                var resultTraining = new Training()
                {
                    User = context.Users.FirstOrDefault(m => m.Id == training.User)
                };

                foreach (var task in training.Tasks)
                {
                    var resultTask = new Task()
                    {
                        Repetition = task.Repetition,
                        Time = task.Time,
                        Exercise = context.Exercises.FirstOrDefault(m => m.Id == task.Exercise)
                    };

                    resultTraining.Tasks.Add(resultTask);
                }

                result.Trainings.Add(resultTraining);
            }

            return result;
        }
    }
}
