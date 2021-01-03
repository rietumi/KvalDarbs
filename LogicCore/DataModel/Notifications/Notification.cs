using System;

namespace LogicCore.DataModel.Notifications
{
    public class Notification : IEntity
    {
        public Notification()
        {
        }

        public Notification(int? teamId, string userId, string message)
        {
            this.Actual = true;
            this.CreationDate = DateTime.Now;
            this.TeamId = teamId;
            this.UserId = userId;
            this.Message = message;
        }

        public int? Id { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime? CreationDate { get; set; }
        public bool Actual { get; set; }
        public string Message { get; set; }
    }
}
