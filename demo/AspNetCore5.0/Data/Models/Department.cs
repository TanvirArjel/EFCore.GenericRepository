using System.Collections.Generic;

namespace AspNetCore5._0.Data.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }
    }
}
