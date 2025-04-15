namespace SothbeysKillerApi.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string Field { get; }
        public string Description { get; }

        public UserNotFoundException(string field, string description)
        {
            Field = field;
            Description = description;
        }
    }
}