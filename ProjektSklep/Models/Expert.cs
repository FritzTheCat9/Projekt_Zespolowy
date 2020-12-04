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
        public int ExpertID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        public ICollection<Product> Products { get; set; }
    }
}
