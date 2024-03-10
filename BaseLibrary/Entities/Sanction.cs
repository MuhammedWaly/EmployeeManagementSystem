using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Sanction :OtherEntityBase
    {
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Punishment { get; set; }
        [Required]
        public DateTime PunishmentDate { get; set; }

        public SanctionType? SanctionType { get; set; }

        public int SanctionTypeId { get; set; }
    }
}
