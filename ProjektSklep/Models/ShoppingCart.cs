using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    public class ShoppingCart
    {
        /* POLA */
        public int PaymentMethodID { get; set; }
        public int ShippingMethodID { get; set; }
        public string DiscountCode { get; set; }
        public List<ShoppingCartElement> ProductList { get; set; }
        public decimal CartPrice { get; set; }

        public decimal countCartPrice()
        {
            decimal sum = 0M;
            if (ProductList != null)
            {
                foreach (var product in ProductList)
                {
                    sum += product.Sum;
                }
            }
            return sum;
        }

        /* METODY */
        public bool AddProduct() 
        { 
            return true; 
        }
        public bool DeleteProduct() 
        { 
            return true; 
        }
    }
}
