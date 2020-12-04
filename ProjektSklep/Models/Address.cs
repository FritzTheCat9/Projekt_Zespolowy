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
        public int AddressID { get; set; }
        [Required]
        public int CustomerID { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string Town { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public string Street { get; set; }
        [Required]
        public int HouseNumber { get; set; }
        public int? ApartmentNumber { get; set; }
    }
}
