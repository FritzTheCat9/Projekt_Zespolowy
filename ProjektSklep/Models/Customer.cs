using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektSklep.Models
{
    [DisplayColumn("CustomerID")]
    public class Customer
    {
        /* POLA */
        [Key]
        public int CustomerID { get; set; }
        [Required]
        [ForeignKey("Address")]
        public int AddressID { get; set; }
        [Required]
        [ForeignKey("PageConfiguration")]
        public int PageConfigurationID { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool AdminRights { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("AddressID")]
        public Address Address { get; set; }
        //[ForeignKey("PageConfigurationID")]
        public PageConfiguration PageConfiguration { get; set; }
        public ICollection<Order> Orders { get; set; }

        /* METODY */
        public bool GenerateNewPassword()
        {
            return true;
        }
    }
}
