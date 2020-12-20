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
        [Display(Name = "KonfiguracjaId")]
        public int PageConfigurationID { get; set; }
        [Required]
        [Display(Name = "KlientId")]
        public string CustomerID { get; set; }
        [Required]
        [Display(Name = "Newsletter")]
        public bool SendingNewsletter { get; set; }
        [Required]
        [Display(Name = "Ceny z NBP")]
        public bool ShowNetPrices { get; set; }
        [Required]
        [Display(Name = "Liczba  produktów na stronie")]
        public int ProductsPerPage { get; set; }
        [Required]
        [Display(Name = "Wygląd strony")]
        public int InterfaceSkin { get; set; }
        [Required]
        [Display(Name = "Język")]
        public int Language { get; set; }
        [Required]
        [Display(Name = "Waluta")]
        public int Currency { get; set; }
    }
}
