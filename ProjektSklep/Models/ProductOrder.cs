using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    [DisplayColumn("ProductOrderID")]
    public class ProductOrder
    {
        /* POLA */
        [Key]
        [Display(Name = "ProductZamowienieId")]
        public int ProductOrderID { get; set; }
        [Required]
        [ForeignKey("Product")]
        [Display(Name = "ProduktId")]
        public int ProductID { get; set; }
        [Required]
        [ForeignKey("Order")]
        [Display(Name = "ZamowienieId")]
        public int OrderID { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("OrderID")]
        [Display(Name = "ZamowienieId")]
        public Order Order { get; set; }
        //[ForeignKey("ProductID")]
        [Display(Name = "ProduktId")]
        public Product Product { get; set; }
    }
}
