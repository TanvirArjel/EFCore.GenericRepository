using EFCore.GenericRepository.Repository;
using System.Threading.Tasks;

namespace EFCore.GenericRepository.UnitOfWork
{
    public interface IUnitOfWork 
    {
        IRepository<T> Repository<T>() where T : class;
        int ExecuteSqlCommand(string sql, params object[] parameters);
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);
        void ResetContextState();
        Task SaveChangesAsync();
    }
}
