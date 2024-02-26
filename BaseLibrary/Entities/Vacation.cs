﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseLibrary.Entities
{
    public class Vacation :OtherEntityBase
    {

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public int NumberOfDays { get; set; }

        public DateTime EndDate => StartDate.AddDays(NumberOfDays);

        public VacationType? VacationType {  get; set; }

        [Required]
        public int VacationTypeId {  get; set; }


    }
}
