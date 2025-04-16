namespace SothbeysKillerApi.Exceptions {
    public class UserUnautorizedException : Exception {
        public string Field { get; }
        public string Description { get; }

        public UserUnautorizedException(string field, string description) {
            Field = field;
            Description = description;
        }
    }
}