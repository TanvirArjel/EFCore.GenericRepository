using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AspNetCoreApp.Data.Models;

public class EmployeeHistory
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long EmployeeId { get; set; }

    public int DepartmentId { get; set; }

    [Required]
    public string EmployeeName { get; set; }
}
