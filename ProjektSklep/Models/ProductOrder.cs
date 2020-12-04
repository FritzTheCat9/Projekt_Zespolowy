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
        public int ProductOrderID { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        [Required]
        [ForeignKey("Order")]
        public int OrderID { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("OrderID")]
        public Order Order { get; set; }
        //[ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
