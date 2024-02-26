using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Branch : BaseEntity
    {
        public Department? Department { get; set; }
        public int DepartmentId { get; set; }
        [JsonIgnore]
        public List<Employee>? Employees { get; set; }
    }
}
