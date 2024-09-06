using System.Threading.Tasks;
using CodeChallenge.Models;
using CodeChallenge.ViewModels;

namespace CodeChallenge.Services
{
    public interface ICompensationService
    {
        Task<CompensationViewModel> GetByEmployeeIdAsync(string id);
        Task<CompensationViewModel> CreateAsync(CompensationViewModel compensationVM);
    }
}
