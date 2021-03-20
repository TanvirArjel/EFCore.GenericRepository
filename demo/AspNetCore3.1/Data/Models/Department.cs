using System.Collections.Generic;

namespace AspNetCore3._1.Data.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
