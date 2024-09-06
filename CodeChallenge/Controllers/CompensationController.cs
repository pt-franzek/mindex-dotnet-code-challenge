using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CodeChallenge.Services;
using CodeChallenge.ViewModels;

namespace CodeChallenge.Controllers
{
    [ApiController]
    [Route("api/compensation")]
    public class CompensationController : ControllerBase
    {
        private readonly ILogger<CompensationController> _logger;
        private readonly ICompensationService _compensationService;

        public CompensationController(ILogger<CompensationController> logger, ICompensationService compensationService)
        {
            _logger = logger;
            _compensationService = compensationService;
        }

        /// <summary>
        /// Creates a new compensation model based on the provided 
        /// body
        /// </summary>
        /// <param name="compensationVM"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateCompensation([FromBody] CompensationViewModel compensationVM)
        {
            _logger.LogDebug("Received compensation create request for '{employeeId}'", compensationVM.EmployeeId);

            // Validate the compensation view model passed
            if (compensationVM is null)
                return BadRequest("Compensation model was null");

            var createdCompensationVM = await _compensationService.CreateAsync(compensationVM);

            return CreatedAtRoute("getCompensationByEmployeeId", new { employeeId = compensationVM.EmployeeId }, createdCompensationVM);
        }

        /// <summary>
        /// Retrieves compensation model for the specified employee
        /// </summary>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        [HttpGet("{employeeId}", Name = "getCompensationByEmployeeId")]
        public async Task<IActionResult> GetCompensationByEmployeeId(string employeeId)
        {
            _logger.LogDebug("Received compensation get request for '{employeeId}'", employeeId);

            var compensationVM = await _compensationService.GetByEmployeeIdAsync(employeeId);

            // Handle null compensation view model
            if (compensationVM == null)
                return NotFound();

            return Ok(compensationVM);
        }
    }
}
