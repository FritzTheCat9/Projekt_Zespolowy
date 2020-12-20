using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    public enum State
    {
        Preparing = 0,
        OnTheWay = 1,
        Delivered = 2
    }

    [DisplayColumn("OrderID")]
    public class Order
    {
        /* POLA */
        [Key]
        [Display(Name = "ZamówienieId")]
        public int OrderID { get; set; }
        [Required]
        [ForeignKey("Customer")]
        [Display(Name = "KlientId")]
        public string CustomerID { get; set; }
        [Required]
        [ForeignKey("ShippingMethod")]
        [Display(Name = "Dostawa")]
        public int ShippingMethodID { get; set; }
        [Required]
        [ForeignKey("PaymentMethod")]
        [Display(Name = "Płatność")]
        public int PaymentMethodID { get; set; }
        [Required]
        [EnumDataType(typeof(State))]
        [Display(Name = "Stan")]
        public State OrderStatus { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        //[ForeignKey("ShippingMethodID")]
        [Display(Name = "Dostawa")]
        public ShippingMethod ShippingMethod { get; set; }
        //[ForeignKey("PaymentMethodID")]
        [Display(Name = "Płatność")]
        public PaymentMethod PaymentMethod { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }

        /* METODY */
        public bool SendOrderStatement()
        {
            return true;
        }
    }
}
