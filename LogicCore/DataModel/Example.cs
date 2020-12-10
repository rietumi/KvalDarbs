namespace LogicCore
{
    public class Example : IEntity
    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

        public Example GetExampleById(int Id)
        {
            throw new System.NotImplementedException();
        }
    }
}