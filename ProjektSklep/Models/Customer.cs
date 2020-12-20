using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ProjektSklep.Models
{
    [DisplayColumn("CustomerID")]
    public class Customer : IdentityUser
    {
        /* POLA */
        /*[Key]
        public int CustomerID { get; set; }*/
        [Required]
        [ForeignKey("Address")]
        [Display(Name = "AdresId")]
        public int AddressID { get; set; }
        [Required]
        [ForeignKey("PageConfiguration")]
        [Display(Name = "KonfiguracjaId")]
        public int PageConfigurationID { get; set; }
        [Required]
        [Display(Name = "Imie")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }
        /*[Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public bool AdminRights { get; set; }*/

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("AddressID")]
        [Display(Name = "AdresId")]
        public Address Address { get; set; }
        //[ForeignKey("PageConfigurationID")]
        [Display(Name = "KonfiguracjaId")]
        public PageConfiguration PageConfiguration { get; set; }
        public ICollection<Order> Orders { get; set; }

        /* METODY */
        public bool GenerateNewPassword()
        {
            return true;
        }
    }
}
