using System.ComponentModel.DataAnnotations;

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

        //public Department Department { get; set; }
    }
}
