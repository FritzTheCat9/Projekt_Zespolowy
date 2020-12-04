using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    public class Admin : Customer
    {
        /* METODY */
        public bool AddProduct(Product p)
        {
            return true;
        }
        public bool DeleteProduct(Product p)
        {
            return true;
        }
        public bool EditProduct(Product p)
        {
            return true;
        }
        public bool HideProduct(Product p)
        {
            return true;
        }
        public bool HideCategory(Category c)
        {
            return true;
        }
        public bool AddCustomer(Customer c)
        {
            return true;
        }
        public bool DeleteCustomer(Customer c)
        {
            return true;
        }
        public bool EditCustomer(Customer c)
        {
            return true;
        }
        public bool SendNewsletter()
        {
            return true;
        }
        public bool ChangeOrderStatus(Order o)
        {
            return true;
        }
    }
}
