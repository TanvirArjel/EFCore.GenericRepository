using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Data.Models
{
    public class Employee
    {
        [Key]
        public long EmployeeId { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string DepartmentName { get; set; }
    }
}
