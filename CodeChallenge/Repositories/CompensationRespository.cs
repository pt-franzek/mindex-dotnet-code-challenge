using System;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using CodeChallenge.Data;

namespace CodeChallenge.Repositories
{
    public class CompensationRespository : ICompensationRepository
    {
        private readonly EmployeeContext _context;
        private readonly ILogger<CompensationRespository> _logger;

        public CompensationRespository(ILogger<CompensationRespository> logger, EmployeeContext employeeContext)
        {
            _context = employeeContext;
            _logger = logger;
        }

        public async Task<Compensation> AddAsync(Compensation compensation)
        {
            if (compensation is null)
                throw new ArgumentNullException(nameof(compensation));

            if (await _context.Compensations.AnyAsync(x => x.EmployeeId == compensation.EmployeeId))
                throw new InvalidOperationException("Cannot add duplicate compensation for the same employee");

            // Generate new compensation GUID and add to DB
            compensation.CompensationId = Guid.NewGuid().ToString();
            await _context.Compensations.AddAsync(compensation);
            return compensation;
        }

        public Task<Compensation> GetByEmployeeIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty");

            return _context.Compensations.SingleOrDefaultAsync(e => e.EmployeeId == id);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
