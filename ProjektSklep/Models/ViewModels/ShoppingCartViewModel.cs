using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models.ViewModels
{
    public class ShoppingCartViewModel
    {
        public ShoppingCart ShoppingCart { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public DiscountCode DiscountCode { get; set; }
    }
}
