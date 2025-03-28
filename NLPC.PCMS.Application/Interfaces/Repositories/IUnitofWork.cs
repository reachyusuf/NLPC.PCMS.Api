namespace NLPC.PCMS.Application.Interfaces.Repositories
{
    public interface IUnitofWork : IDisposable
    {
        IGenericRepository<T> Repository<T>() where T : class;
        Task<int> SaveChanges();
        void Rollback();
    }
}
