﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore3._1.Data;
using AspNetCore3._1.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TanvirArjel.EFCore.GenericRepository;

namespace AspNetCore3._1.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IRepository<DemoDbContext> _repository01;
        private readonly IRepository<DemoDbContext2> _repository02;
        private readonly DemoDbContext _context;

        public EmployeeController(
            IRepository<DemoDbContext> repository01,
            DemoDbContext context,
            IRepository<DemoDbContext2> repository02)
        {
            _repository01 = repository01;
            _context = context;
            _repository02 = repository02;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            List<Department> departments = await _context.Set<Department>()
                .Where(d => d.Employees.Any(e => e.EmployeeName == "Tanvir Ahmad")).ToListAsync();

            await _repository01.GetLongCountAsync<Employee>();
            Specification<Employee> specification = new Specification<Employee>();
            specification.Conditions.Add(e => e.EmployeeName.Contains("Tanvir"));
            specification.Includes = ep => ep.Include(e => e.Department);
            specification.OrderBy = sp => sp.OrderBy(e => e.EmployeeName).ThenBy(e => e.DepartmentName);
            specification.Skip = 0;
            specification.Take = 10;

            List<Employee> lists = _repository01.GetQueryable<Employee>().ToList();
            Employee entityListAsync = await _repository01.GetByIdAsync<Employee>(1, true);



            Employee v1 = await _repository01.GetByIdAsync<Employee>(1);

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
            Employee employee = await _repository01.GetByIdAsync<Employee>(id);
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
                await _repository01.InsertAsync(employee);
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

            Employee employee = await _repository01.GetByIdAsync<Employee>(id);
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
                Employee employeeToBeUpdated = await _repository01.GetByIdAsync<Employee>(employee.EmployeeId);
                employeeToBeUpdated.EmployeeName = employee.EmployeeName;
                employeeToBeUpdated.DepartmentName = employee.DepartmentName;
                await _repository01.UpdateAsync(employeeToBeUpdated);
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

            Employee employee = await _repository01.GetByIdAsync<Employee>(id);
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
            Employee employee = await _repository01.GetByIdAsync<Employee>(id);
            await _repository01.DeleteAsync(employee);
            return RedirectToAction(nameof(Index));
        }

    }
}
