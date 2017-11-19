using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevExpressASPNETCoreReporting.Models
{
    public class ProdUnit
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        [Required]
        public int UnitId { get; set; }
        public virtual Unit Unit { get; set; }

        public bool IsCore { get; set; }
    }
}
