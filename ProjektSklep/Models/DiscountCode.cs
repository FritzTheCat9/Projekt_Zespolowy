using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjektSklep.Models
{
    public class DiscountCode
    {
        /* POLA */
        [Key]
        public int DiscountCodeID { get; set; }
        [Required]
        public string DiscoundCode { get; set; }
        [Required]
        public int Percent { get; set; }
    }
}
