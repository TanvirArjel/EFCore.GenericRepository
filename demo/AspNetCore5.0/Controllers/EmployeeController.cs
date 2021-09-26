using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using AspNetCore5._0.Data;
using AspNetCore5._0.Data.Models;
using AspNetCore5._0.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TanvirArjel.EFCore.GenericRepository;

namespace AspNetCore5._0.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository _repository;
        private readonly IQueryRepository _queryRepository;
        private readonly DemoDbContext _context;

        public EmployeeController(
            IRepository repository,
            IQueryRepository queryRepository,
            DemoDbContext context)
        {
            _repository = repository;
            _context = context;
            _queryRepository = queryRepository;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            //var list = _context.SqlQuery(
            //    () => new { EmployeeName = "", DepartmentName = "" },
            //    "Select EmployeeName,DepartmentName from Employee Where EmployeeId = @p0", 1);
            List<string> search = new List<string>() { "Tanvir", "Software" };
            string sqlQuery = "Select EmployeeName, DepartmentName from Employee Where EmployeeName LIKE @p0 + '%' and DepartmentName LIKE @p1 + '%'";
            List<EmployeeDto> items = await _repository
                .GetFromRawSqlAsync<EmployeeDto>(sqlQuery, search);

            //List<string> list1 = _context.ExecSQL<string>("Select EmployeeName from Employee");
            List<Employee> lists = await _queryRepository.GetQueryable<Employee>().ToListAsync();
            return View(lists);
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
