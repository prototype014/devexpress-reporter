using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevExpressASPNETCoreReporting.Models
{
    public class Organisation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(10)]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(255)]
        [Display(Name = "Name")]
        public string Name { get; set; }
    }

}


