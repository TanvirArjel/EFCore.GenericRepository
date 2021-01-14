using System.ComponentModel.DataAnnotations;

namespace AspNetCore5._0.Data.Models
{
    public class Employee
    {
        [Key]
        public long EmployeeId { get; set; }

        public int DepartmentId { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        public string DepartmentName { get; set; }

        public Department Department { get; set; }
    }
}
