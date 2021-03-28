using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore5._0.Data;
using AspNetCore5._0.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TanvirArjel.EFCore.GenericRepository;

namespace AspNetCore5._0.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository _repository;
        private readonly DemoDbContext _context;

        public EmployeeController(
            IRepository repository,
            DemoDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {

            ////List<Department> departments = await _context.Set<Department>()
            ////    .Where(d => d.Employees.Any(e => e.EmployeeName == "Tanvir Ahmad")).ToListAsync();

            ////Specification<Department> specification = new Specification<Department>();
            ////specification.Conditions.Add(d => d.Employees.Any(e => e.EmployeeName == "Tanvir Ahmad"));

            ////List<string> departments1 = await _repository.GetListAsync(specification, d => d.Name);

            ////PaginatedList<Employee> paginatedList = await _repository.GetQueryable<Employee>().ToPaginatedListAsync(1, 10);

            IQueryable<EmployeeDto> queryable = _repository.GetQueryable<Employee>()
                .Where(e => e.EmployeeName.Contains("Ta")).Select(e => new EmployeeDto
                {
                    EmployeeName = e.EmployeeName,
                    DepartmentName = e.DepartmentName
                }).OrderBy(e => e.DepartmentName);

            PaginationSpecification<Employee> specification = new PaginationSpecification<Employee>();
            specification.Conditions.Add(e => e.EmployeeName.Contains("Ta"));
            specification.Includes = q => q.Include(e => e.Department);
            specification.PageIndex = 1;
            specification.PageSize = 2;

            PaginatedList<Employee> paginatedList1 = await _repository.GetPaginatedListAsync(specification);

            List<Employee> lists = _repository.GetQueryable<Employee>().ToList();
            return View(lists);
        }

        public class EmployeeDto
        {
            public string EmployeeName { get; set; }

            public string DepartmentName { get; set; }
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _repository.GetByIdAsync<Employee>(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                IDbContextTransaction transaction = await _repository.BeginTransactionAsync(IsolationLevel.ReadCommitted);

                try
                {
                    employee.DepartmentId = 1;

                    object[] primaryKeys = await _repository.InsertAsync(employee);


                    long employeeId = (long)primaryKeys[0];
                    EmployeeHistory employeeHistory = new EmployeeHistory()
                    {
                        EmployeeId = employeeId,
                        DepartmentId = employee.DepartmentId,
                        EmployeeName = employee.EmployeeName
                    };

                    await _repository.InsertAsync(employeeHistory);

                    await transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                }

                return RedirectToAction(nameof(Index));

            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = await _repository.GetByIdAsync<Employee>(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, Employee employee)
        {
            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                Employee employeeToBeUpdated = await _repository.GetByIdAsync<Employee>(employee.EmployeeId);
                employeeToBeUpdated.EmployeeName = employee.EmployeeName;
                employeeToBeUpdated.DepartmentName = employee.DepartmentName;
                await _repository.UpdateAsync(employeeToBeUpdated);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Employee employee = await _repository.GetByIdAsync<Employee>(id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            Employee employee = await _repository.GetByIdAsync<Employee>(id);
            await _repository.DeleteAsync(employee);
            return RedirectToAction(nameof(Index));
        }

    }
}
