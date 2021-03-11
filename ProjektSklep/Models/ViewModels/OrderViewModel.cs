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
        public List<Product> ProductList { get; set; }
        public State OrderStatus { get; set; }
        public decimal Price { get; set; }
        public Dictionary<Product, int> ProductIdQuantity { get; set; }

        public OrderViewModel(int id, Customer customer, PaymentMethod paymentMethod, ShippingMethod shippingMethod, State orderStatus, decimal price)
        {
            this.id = id;
            this.Customer = customer;
            this.PaymentMethod = paymentMethod;
            this.ShippingMethod = shippingMethod;
            this.OrderStatus = orderStatus;
            this.Price = price;
            ProductList = new List<Product>();
            ProductIdQuantity = new Dictionary<Product, int>();
        }

        public void addProduct(Product p)
        {
            ProductList.Add(p);
        }

        public void addProductIdQuantity(Product p, int quantity)
        {
            /*bool keyExists = ProductIdQuantity.ContainsKey(p);
            if (keyExists)
            {
                ProductIdQuantity[p] = ProductIdQuantity[p] + quantity;
            }
            else
            {
                ProductIdQuantity.Add(p, quantity);
            }*/
            ProductIdQuantity.Add(p, quantity);
        }
    }
}
