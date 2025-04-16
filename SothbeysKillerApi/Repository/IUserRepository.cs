using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Repository {
    public interface IUserRepository {
        bool EmailExist(string email);
        void Create(User entity);
        User? Signin(string email);

    }
}