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
        [Display(Name = "Id")]
        public int DiscountCodeID { get; set; }
        [Required]
        [Display(Name = "Kod rabatowy")]
        public string DiscoundCode { get; set; }
        [Required]
        [Display(Name = "Procent")]
        public int Percent { get; set; }
    }
}
