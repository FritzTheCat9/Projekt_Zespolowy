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
        public int OrderID { get; set; }
        [Required]
        [ForeignKey("Customer")]
        public int CustomerID { get; set; }
        [Required]
        [ForeignKey("ShippingMethod")]
        public int ShippingMethodID { get; set; }
        [Required]
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodID { get; set; }
        [Required]
        [EnumDataType(typeof(State))]
        public State OrderStatus { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("CustomerID")]
        public Customer Customer { get; set; }
        //[ForeignKey("ShippingMethodID")]
        public ShippingMethod ShippingMethod { get; set; }
        //[ForeignKey("PaymentMethodID")]
        public PaymentMethod PaymentMethod { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }

        /* METODY */
        public bool SendOrderStatement()
        {
            return true;
        }
    }
}
