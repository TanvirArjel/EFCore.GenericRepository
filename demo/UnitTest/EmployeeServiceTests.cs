using AspNetCoreApp.Data.Models;
using AspNetCoreApp.Dtos;
using AspNetCoreApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using Xunit;

namespace UnitTest
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IRepository> mockRepository = new Mock<IRepository>();

        private EmployeeService EmployeeService => new EmployeeService(mockRepository.Object);

        [Fact]
        public async Task GetPaginatedListAsync_WithNoParams_ReturnsListOfEmployeeDto()
        {
            // Arrange
            List<EmployeeDto> fakeEmployeeList = new List<EmployeeDto>()
            {
                new EmployeeDto { EmployeeName = "Tanvir", DepartmentName = "Software" }
            };

            mockRepository.Setup(mr => mr.GetListAsync(
                It.IsAny<Specification<Employee>>(),
                It.IsAny<Expression<Func<Employee, EmployeeDto>>>(),
                It.IsAny<CancellationToken>())).ReturnsAsync(fakeEmployeeList);

            // Act

            List<EmployeeDto> employees = await EmployeeService.GetPaginatedListAsync();

            // Assert

            Assert.Single(employees);
        }
    }
}
