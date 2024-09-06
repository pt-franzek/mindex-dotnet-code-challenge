using System;
using System.Threading.Tasks;
using CodeChallenge.Models;
using Microsoft.Extensions.Logging;
using CodeChallenge.Repositories;
using CodeChallenge.ViewModels;

namespace CodeChallenge.Services
{
    public class CompensationService : ICompensationService
    {
        private readonly ICompensationRepository _compensationRepository;
        private readonly ILogger<CompensationService> _logger;

        public CompensationService(ILogger<CompensationService> logger, ICompensationRepository compensationRepository)
        {
            _compensationRepository = compensationRepository;
            _logger = logger;
        }

        public async Task<CompensationViewModel> CreateAsync(CompensationViewModel compensationVM)
        {
            if (compensationVM is null)
                throw new ArgumentNullException(nameof(compensationVM));

            // Transform view model into the domain model
            var compensation = new Compensation
            {
                EmployeeId = compensationVM.EmployeeId,
                Salary = compensationVM.Salary,
                EffectiveDate = compensationVM.EffectiveDate
            };

            // Add Compensation model to DB and save changes
            await _compensationRepository.AddAsync(compensation);
            await _compensationRepository.SaveAsync();

            // Return user facing view model 
            return new CompensationViewModel(compensation.EmployeeId, compensation.Salary, compensation.EffectiveDate);
        }

        public async Task<CompensationViewModel> GetByEmployeeIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentException("ID cannot be null or empty");

            // Retrieve Compensation by Employee ID 
            var compensation = await _compensationRepository.GetByEmployeeIdAsync(id);

            if (compensation is null)
                return null;

            // Transform to user facing view model
            return new CompensationViewModel(compensation.EmployeeId, compensation.Salary, compensation.EffectiveDate);
        }
    }
}
