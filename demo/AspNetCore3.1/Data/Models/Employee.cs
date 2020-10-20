using System.ComponentModel.DataAnnotations;

namespace AspNetCore3._1.Data.Models
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
