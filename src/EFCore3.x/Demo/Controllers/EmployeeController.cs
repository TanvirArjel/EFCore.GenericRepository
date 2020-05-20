using Demo.Data;
using Demo.Data.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;
using System.Collections.Generic;

namespace Demo.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository _repository;
        private readonly DemoDbContext _context;
        public EmployeeController(IUnitOfWork unitOfWork, IRepository repository, DemoDbContext context)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            await _unitOfWork.Repository<Employee>().GetLongCountAsync();
            Specification<Employee> specification = new Specification<Employee>();
            //specification.Conditions.Add(e => e.EmployeeName.Contains("Tanvir"));
            //specification.Includes = ep => ep.Include(e => e.Department);
            //specification.OrderBy = sp => sp.OrderBy(e => e.EmployeeName).ThenBy(e => e.DepartmentName);
            //specification.Skip = 0;
            //specification.Take = 10;

            List<Employee> lists = _repository.GetQueryable<Employee>().ToList();
            Employee entityListAsync = await _repository.GetEntityByIdAsync<Employee>(1, true);



            long v1 = await _repository.GetProjectedEntityByIdAsync<Employee, long>(1, e => e.EmployeeId);

            await _context.Set<Employee>().Where(e => e.EmployeeId == 1).ToListAsync();

            _context.Set<Employee>().Where(e => e.EmployeeId == 1).ToList();
            //await _unitOfWork.Repository<Employee>().GetEn
            return View(lists);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Employee employee = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(id);
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
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,DepartmentName")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Repository<Employee>().InsertEntityAsync(employee);
                await _unitOfWork.SaveChangesAsync();
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

            Employee employee = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(id);
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
        public async Task<IActionResult> Edit(long id, [Bind("EmployeeId,EmployeeName,DepartmentName")] Employee employee)
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
                Employee employeeToBeUpdated = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(employee.EmployeeId);
                employeeToBeUpdated.EmployeeName = employee.EmployeeName;
                employeeToBeUpdated.DepartmentName = employee.DepartmentName;
                _unitOfWork.Repository<Employee>().UpdateEntity(employeeToBeUpdated);
                await _unitOfWork.SaveChangesAsync();
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

            Employee employee = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(id);
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
            Employee employee = await _unitOfWork.Repository<Employee>().GetEntityByIdAsync(id);
            _unitOfWork.Repository<Employee>().DeleteEntity(employee);
            await _unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
}
