using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Doctor :OtherEntityBase
    {
        [Required]
        public DateTime Date {  get; set; }

        [Required]
        public string MidicalDiagnose { get; set; } = string.Empty;

        [Required]
        public string MidicalRecommendation { get; set; } = string.Empty;
    }
}
