using CodeChallenge.Models;
using System.Threading.Tasks;

namespace CodeChallenge.Repositories
{
    public interface ICompensationRepository
    {
        Task<Compensation> GetByEmployeeIdAsync(string id);
        Task<Compensation> AddAsync(Compensation compensation);
        Task SaveAsync();
    }
}