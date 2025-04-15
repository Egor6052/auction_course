namespace SothbeysKillerApi.Repository
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        void Commit();

        void Rollback();
    }
}