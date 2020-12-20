using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    [DisplayColumn("AddressID")]
    public class Address
    {
        /* POLA */
        [Key]
        [Display(Name = "AdresId")]
        public int AddressID { get; set; }
        [Required]
        [Display(Name = "KlientId")]
        public string CustomerID { get; set; }
        [Required]
        [Display(Name = "Kraj")]
        public string Country { get; set; }
        [Required]
        [Display(Name = "Miasto")]
        public string Town { get; set; }
        [Required]
        [Display(Name = "Kod pocztowy")]
        public string PostCode { get; set; }
        [Required]
        [Display(Name = "Ulica")]
        public string Street { get; set; }
        [Required]
        [Display(Name = "Numer domu")]
        public int HouseNumber { get; set; }
        [Display(Name = "Numer apartamentu")]
        public int? ApartmentNumber { get; set; }
    }
}
