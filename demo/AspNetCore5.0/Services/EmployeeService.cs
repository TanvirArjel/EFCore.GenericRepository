using AspNetCore5._0.Data.Models;
using AspNetCore5._0.Dtos;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TanvirArjel.EFCore.GenericRepository;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace AspNetCore5._0.Services
{
    public class EmployeeService : IScopedService
    {
        private readonly IRepository _repository;

        public EmployeeService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<EmployeeDto>> GetPaginatedListAsync()
        {
            Specification<Employee> specification = new Specification<Employee>();
            specification.Conditions.Add(e => e.EmployeeName.Contains("Ta"));
            specification.Includes = q => q.Include(e => e.Department);
            specification.OrderBy = q => q.OrderBy(e => e.EmployeeName);
            specification.Skip = 0;
            specification.Take = 4;

            long count = await _repository.GetLongCountAsync(specification.Conditions);

            List<EmployeeDto> paginatedList = await _repository.GetListAsync(specification, e => new EmployeeDto
            {
                EmployeeName = e.EmployeeName,
                DepartmentName = e.DepartmentName
            });

            return paginatedList;
        }
    }
}
