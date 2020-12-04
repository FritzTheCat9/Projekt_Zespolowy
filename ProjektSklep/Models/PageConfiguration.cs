using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    [DisplayColumn("PageConfigurationID")]
    public class PageConfiguration
    {
        /* POLA */
        [Key]
        public int PageConfigurationID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public bool SendingNewsletter { get; set; }
        [Required]
        public bool ShowNetPrices { get; set; }
        [Required]
        public int ProductsPerPage { get; set; }
        [Required]
        public int InterfaceSkin { get; set; }
        [Required]
        public int Language { get; set; }
        [Required]
        public int Currency { get; set; }
    }
}
