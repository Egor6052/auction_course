namespace SothbeysKillerApi.Exceptions {
    public class UserValidationExceprion : Exception {
        public string Field { get; }
        public string Description { get; }

        public UserValidationExceprion(string field, string description) {
            Field = field;
            Description = description;
        }
    }
}