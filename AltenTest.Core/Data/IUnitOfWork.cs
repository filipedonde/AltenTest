using System.Threading.Tasks;

namespace AltenTest.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}