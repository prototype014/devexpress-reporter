using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevExpressASPNETCoreReporting.Models
{
    public class Product
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
        public virtual Organisation Organisation { get; set; }

        [DataType(DataType.Text), MaxLength(256)]
        [Display(Name = "ANZSCO identifier")]
        public string AV_ANZSCO { get; set; }

        [DataType(DataType.Text), MaxLength(256)]
        [Display(Name = "ASCED Field of Education identifier ")]
        public string AV_ASCED { get; set; }

        [DataType(DataType.Text), MaxLength(256)]
        [Display(Name = "Level of Education identifier ")]
        public string AV_Level { get; set; }

        [DataType(DataType.Text), MaxLength(256)]
        [Display(Name = "Recognition identifier ")]
        public string AV_Recognition { get; set; }

        public virtual List<ProdUnit> Units { get; set; }
    }
}