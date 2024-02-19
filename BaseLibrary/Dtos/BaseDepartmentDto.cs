using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Dtos
{
    public class BaseDepartmentDto
    {
        public int Id { get; set; }
        [Required,MinLength(3),MaxLength(50)]
        public string Name { get; set; }
    }
}
