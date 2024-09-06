
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CodeChallenge.ViewModels;
using CodeCodeChallenge.Tests.Integration.Extensions;
using CodeCodeChallenge.Tests.Integration.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeChallenge.Tests.Integration
{
    [TestClass]
    public class CompensationControllerTests
    {
        private static HttpClient _httpClient;
        private static TestServer _testServer;

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            _testServer = new TestServer();
            _httpClient = _testServer.NewClient();
        }

        [ClassCleanup]
        public static void CleanUpTest()
        {
            _httpClient.Dispose();
            _testServer.Dispose();
        }

        [TestMethod]
        public async Task CreateCompensation_Returns_Created()
        {
            // Arrange
            var compensationVM = new CompensationViewModel("b7839309-3348-463b-a7e3-5de1c168beb3", 
                100000, DateTime.Now.Date);

            // Execute
            var response = await _httpClient.PostAsJsonAsync("api/compensation", compensationVM);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);

            var newCompensation = response.DeserializeContent<CompensationViewModel>();
            Assert.AreEqual(compensationVM.EmployeeId, newCompensation.EmployeeId);
            Assert.AreEqual(compensationVM.Salary, newCompensation.Salary);
            Assert.AreEqual(compensationVM.EffectiveDate, newCompensation.EffectiveDate);
        }

        [TestMethod]
        public async Task CreateCompensation_Returns_InternalServerError()
        {
            // Arrange
            var compensationVM = new CompensationViewModel("16a596ae-edd3-4847-99fe-c4518e82c86f", 
                150000, DateTime.Now.Date);

            var dupCompensationVM = new CompensationViewModel("16a596ae-edd3-4847-99fe-c4518e82c86f", 
                175000, DateTime.Now.Date);

            // Execute
            await _httpClient.PostAsJsonAsync("api/compensation", compensationVM);
            var response = await _httpClient.PostAsJsonAsync("api/compensation", dupCompensationVM);

            // Assert
            Assert.AreEqual(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.IsNotNull(await response.Content.ReadAsStringAsync());
        }

        [TestMethod]
        public async Task GetCompensationByEmployeeId_Returns_CompensationViewModel() 
        {
            // Arrange
            var employeeId = "03aa1462-ffa9-4978-901b-7c001562cf6f";

            // Create initial Compensation to retrieve with Get request
            var compensationVM = new CompensationViewModel(employeeId, 125000, DateTime.Now.Date);
            await _httpClient.PostAsJsonAsync("api/compensation", compensationVM);

            // Execute
            var response = await _httpClient.GetAsync($"api/compensation/{employeeId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);

            var compensation = response.DeserializeContent<CompensationViewModel>();
            Assert.AreEqual(compensationVM.EmployeeId, compensation.EmployeeId);
            Assert.AreEqual(compensationVM.Salary, compensation.Salary);
            Assert.AreEqual(compensationVM.EffectiveDate, compensation.EffectiveDate);
        }

        [TestMethod]
        public async Task GetCompensationByEmployeeId_Returns_NotFound()
        {
            // Arrange
            var employeeId = "abc";

            // Execute
            var response = await _httpClient.GetAsync($"api/compensation/{employeeId}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
