using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models.ViewModels
{
    public class OrderViewModel
    {
        public int id { get; set; }
        public Customer Customer { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public ShippingMethod ShippingMethod { get; set; }
        public List<Product> Order { get; set; }
        public State OrderStatus { get; set; }
        public decimal Price { get; set; }

        public OrderViewModel(int id, Customer customer, PaymentMethod paymentMethod, ShippingMethod shippingMethod, State orderStatus, decimal price)
        {
            this.id = id;
            this.Customer = customer;
            this.PaymentMethod = paymentMethod;
            this.ShippingMethod = shippingMethod;
            this.OrderStatus = orderStatus;
            this.Price = price;
            Order = new List<Product>();
        }

        public void addProduct(Product p)
        {
            Order.Add(p);
        }

    }
}
