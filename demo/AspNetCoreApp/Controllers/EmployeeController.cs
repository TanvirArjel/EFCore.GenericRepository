using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreApp.Data;
using AspNetCoreApp.Data.Models;
using AspNetCoreApp.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace AspNetCoreApp.Controllers;

public class EmployeeController : Controller
{
    private readonly IRepository _repository;
    private readonly IQueryRepository _queryRepository;
    private readonly Demo1DbContext _context;
    private readonly IRepository<Demo1DbContext> _demo1Repository;

    public EmployeeController(
        IRepository repository,
        IQueryRepository queryRepository,
        Demo1DbContext context,
        IRepository<Demo1DbContext> demo1Repository)
    {
        _repository = repository;
        _context = context;
        _queryRepository = queryRepository;
        _demo1Repository = demo1Repository;
    }

    // GET: Employee
    public async Task<IActionResult> Index()
    {
        //var list = _context.SqlQuery(
        //    () => new { EmployeeName = "", DepartmentName = "" },
        //    "Select EmployeeName,DepartmentName from Employee Where EmployeeId = @p0", 1);
        List<string> search = new List<string>() { "Tanvir", "Software" };
        string sqlQuery = "Select EmployeeName, DepartmentName from Employee Where EmployeeName LIKE @p0 + '%' and DepartmentName LIKE @p1 + '%'";
        List<EmployeeDto> items = await _repository.GetFromRawSqlAsync<EmployeeDto>(sqlQuery, search);

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
        bool isExistent = await _repository.ExistsByIdAsync<Employee>(id);
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
            // Insert to database 1
            employee.DepartmentId = 1;

            _repository.Add(employee);

            EmployeeHistory employeeHistory1 = new EmployeeHistory()
            {
                EmployeeId = employee.EmployeeId,
                DepartmentId = employee.DepartmentId,
                EmployeeName = employee.EmployeeName
            };

            _repository.Add(employeeHistory1);
            await _repository.SaveChangesAsync();

            // Insert to database 2
            employee.EmployeeId = 0;
            employee.DepartmentId = 1;

            EmployeeHistory employeeHistory2 = new EmployeeHistory()
            {
                EmployeeId = employee.EmployeeId,
                DepartmentId = employee.DepartmentId,
                EmployeeName = employee.EmployeeName
            };

            _demo1Repository.Add(employee);
            _demo1Repository.Add(employeeHistory2);
            _demo1Repository.Remove(employee);
            await _demo1Repository.SaveChangesAsync();

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

            _repository.Update(employeeToBeUpdated);
            await _repository.SaveChangesAsync();

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
        Employee employee = await _queryRepository.GetByIdAsync<Employee>(id);
        _repository.Remove(employee);
        await _repository.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

}
