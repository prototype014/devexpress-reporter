using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevExpressASPNETCoreReporting.Models
{
    public class Unit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(20)]
        public string Code { get; set; }

        [Required]
        [DataType(DataType.Text), MaxLength(256)]
        public string Name { get; set; }

        [Display(Name = "Organisation")]
        public int? OrganisationId { get; set; }
        public Organisation Organisation { get; set; }
    }
}