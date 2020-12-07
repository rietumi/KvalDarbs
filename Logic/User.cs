namespace Logic
{
    public class User : IEntity
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public int Id { get; set; }

        public UserType Type { get; set; }

        public int Email
        {
            get => default;
            set
            {
            }
        }

        public int Reminder
        {
            get => default;
            set
            {
            }
        }
    }
}