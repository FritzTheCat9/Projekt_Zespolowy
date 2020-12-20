using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ProjektSklep.Models
{
    [DisplayColumn("ExpertID")]
    public class Expert
    {
        /* POLA */
        [Key]
        [Display(Name = "EkspertId")]
        public int ExpertID { get; set; }
        [Required]
        [Display(Name = "Imie")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        public ICollection<Product> Products { get; set; }
    }
}
