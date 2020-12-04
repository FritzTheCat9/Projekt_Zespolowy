using ProjektSklep.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ShopContext context)
        {
            context.Database.EnsureCreated();

            /* DiscountCodes */
            if (context.DiscountCodes.Any())
            {
                return;
            }
            var discountCodes = new DiscountCode[]
            {
                new DiscountCode{ DiscoundCode="123ABC", Percent=50 },
                new DiscountCode{ DiscoundCode="234BCD", Percent=30 },
                new DiscountCode{ DiscoundCode="123ABC", Percent=10 },
                new DiscountCode{ DiscoundCode="123XYZ", Percent=20 },
                new DiscountCode{ DiscoundCode="789KLM", Percent=70 }
            };

            foreach (DiscountCode discountCode in discountCodes)
            {
                context.DiscountCodes.Add(discountCode);
            }
            context.SaveChanges();

            /* Addresses */
            if (context.Addresses.Any())
            {
                return;
            }
            var addresses = new Address[]
            {
                new Address{ CustomerID=1, Country="Polska", Town="Białystok", PostCode="12-123", Street="Wesoła", HouseNumber=123, ApartmentNumber=1 },
                new Address{ CustomerID=2, Country="Polska", Town="Warszawa", PostCode="23-456", Street="Piękna", HouseNumber=12, ApartmentNumber=47 },
                new Address{ CustomerID=3, Country="Polska", Town="Gdańsk", PostCode="56-678", Street="Miła", HouseNumber=56, ApartmentNumber=23 },
                new Address{ CustomerID=4, Country="Polska", Town="Kraków", PostCode="78-234", Street="Diamentowa", HouseNumber=26, },
                new Address{ CustomerID=5, Country="Polska", Town="Łomża", PostCode="34-785", Street="Wiejska", HouseNumber=6, ApartmentNumber=67 }
            };

            foreach (Address address in addresses)
            {
                context.Addresses.Add(address);
            }
            context.SaveChanges();

            /* PageConfigurations */
            if (context.PageConfigurations.Any())
            {
                return;
            }
            var pageConfigurations = new PageConfiguration[]
            {
                new PageConfiguration{ CustomerID=1, SendingNewsletter=false, ShowNetPrices=true, ProductsPerPage=20, InterfaceSkin=0, Language=0, Currency=1 },
                new PageConfiguration{ CustomerID=2, SendingNewsletter=true, ShowNetPrices=false, ProductsPerPage=30, InterfaceSkin=1, Language=1, Currency=0 },
                new PageConfiguration{ CustomerID=3, SendingNewsletter=false, ShowNetPrices=false, ProductsPerPage=15, InterfaceSkin=0, Language=0, Currency=3 },
                new PageConfiguration{ CustomerID=4, SendingNewsletter=true, ShowNetPrices=false, ProductsPerPage=10, InterfaceSkin=1, Language=0, Currency=2 },
                new PageConfiguration{ CustomerID=5, SendingNewsletter=true, ShowNetPrices=true, ProductsPerPage=50, InterfaceSkin=0, Language=1, Currency=0 }
            };

            foreach (PageConfiguration pageConfiguration in pageConfigurations)
            {
                context.PageConfigurations.Add(pageConfiguration);
            }
            context.SaveChanges();

            /* Customers */
            if (context.Customers.Any())
            {
                return;
            }
            var customers = new Customer[]
            {
                new Customer{ AddressID=1, PageConfigurationID=1, FirstName="Bartłomiej", LastName="Umiński", Login="bartek", Password="uminski", Email="bartlomiejuminski1999@gmai.com", AdminRights=true },
                new Customer{ AddressID=2, PageConfigurationID=2, FirstName="Kacper", LastName="Siegieńczuk", Login="kacper", Password="siegienczuk", Email="kacpersiegienczuk@gmai.com", AdminRights=true },
                new Customer{ AddressID=3, PageConfigurationID=3, FirstName="Michał", LastName="Kozikowski", Login="michal", Password="kozikowski", Email="michalkozikowski@gmai.com", AdminRights=true },
                new Customer{ AddressID=4, PageConfigurationID=4, FirstName="Jakub", LastName="Kozłowski", Login="jakub", Password="kozlowski", Email="jakubkozlowski@gmai.com", AdminRights=true },
                new Customer{ AddressID=5, PageConfigurationID=5, FirstName="Klient", LastName="Klientowski", Login="klient", Password="klient", Email="klientklientowski@gmai.com", AdminRights=false }
            };

            foreach (Customer customer in customers)
            {
                context.Customers.Add(customer);
            }
            context.SaveChanges();

            /* ShippingMethods */
            if (context.ShippingMethods.Any())
            {
                return;
            }
            var shippingMethods = new ShippingMethod[]
            {
                new ShippingMethod{ Name="Kurier UPS" },
                new ShippingMethod{ Name="Kurier DHL" },
                new ShippingMethod{ Name="Poczta" },
                new ShippingMethod{ Name="Paczkomat Inpost" }
            };

            foreach (ShippingMethod shippingMethod in shippingMethods)
            {
                context.ShippingMethods.Add(shippingMethod);
            }
            context.SaveChanges();

            /* PaymentMethods */
            if (context.PaymentMethods.Any())
            {
                return;
            }
            var paymentMethods = new PaymentMethod[]
            {
                new PaymentMethod{ Name="Przelew Tradycyjny" },
                new PaymentMethod{ Name="Przelew Blik" },
                new PaymentMethod{ Name="Szybki Przelew" },
                new PaymentMethod{ Name="Karta" }
            };

            foreach (PaymentMethod paymentMethod in paymentMethods)
            {
                context.PaymentMethods.Add(paymentMethod);
            }
            context.SaveChanges();

            /* Orders */
            if (context.Orders.Any())
            {
                return;
            }
            var orders = new Order[]
            {
                new Order{ CustomerID=1, ShippingMethodID=1, PaymentMethodID=3, OrderStatus=State.Preparing },
                new Order{ CustomerID=2, ShippingMethodID=2, PaymentMethodID=4, OrderStatus=State.OnTheWay },
                new Order{ CustomerID=3, ShippingMethodID=2, PaymentMethodID=2, OrderStatus=State.Delivered },
                new Order{ CustomerID=4, ShippingMethodID=3, PaymentMethodID=1, OrderStatus=State.Delivered },
                new Order{ CustomerID=5, ShippingMethodID=4, PaymentMethodID=4, OrderStatus=State.OnTheWay }
            };

            foreach (Order order in orders)
            {
                context.Orders.Add(order);
            }
            context.SaveChanges();

            /* Experts */
            if (context.Experts.Any())
            {
                return;
            }
            var experts = new Expert[]
            {
                new Expert{ FirstName="Ekspert", LastName="Ekspertowicz", Email="ekspercik@gmail.com"},
                new Expert{ FirstName="Ekspert2", LastName="Ekspertowicz2", Email="ekspercik2@gmail.com"},
                new Expert{ FirstName="Ekspert3", LastName="Ekspertowicz3", Email="ekspercik3@gmail.com"}
            };

            foreach (Expert expert in experts)
            {
                context.Experts.Add(expert);
            }
            context.SaveChanges();

            /* Categories */
            if (context.Categories.Any())
            {
                return;
            }
            var categories = new Category[]
            {
                new Category{ Name="Elektronika", Visibility=true },
                new Category{ ParentCategoryID=1, Name="Smartfony", Visibility=true },
                new Category{ ParentCategoryID=1, Name="Laptopy", Visibility=true },
                new Category{ ParentCategoryID=1, Name="Komputery", Visibility=false}
            };

            foreach (Category category in categories)
            {
                context.Categories.Add(category);
            }
            context.SaveChanges();

            /* Products */
            if (context.Products.Any())
            {
                return;
            }
            var products = new Product[]
            {
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop LENOVO", ProductDescription="Dobry laptop", Image="~/Content/Images/Products/Laptop LENOVO.jpg", DateAdded=new DateTime(), Promotion=false, VAT=23, Price=4300, Amount=20, Visibility=true, SoldProducts=13 },
                new Product{ CategoryID=3, ExpertID=1, Name="Smartfon HUAWEI P30", ProductDescription="Dobry smartfon", Image="~/Content/Images/Products/Smartfon HUAWEI P30.jpg", DateAdded=new DateTime(), Promotion=true, VAT=23, Price=2999, Amount=10, Visibility=true, SoldProducts=5 },
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop HUAWEI", ProductDescription="Dobry laptop", Image="~/Content/Images/Products/Laptop HUAWEI.png", DateAdded=new DateTime(), Promotion=true, VAT=23, Price=5000, Amount=34, Visibility=true, SoldProducts=7 },
                new Product{ CategoryID=2, ExpertID=1, Name="Laptop APPLE", ProductDescription="Dobry laptop", Image="~/Content/Images/Products/Laptop APPLE.jpg", DateAdded=new DateTime(), Promotion=false, VAT=23, Price=4000, Amount=56, Visibility=true, SoldProducts=8 },
            };

            foreach (Product product in products)
            {
                context.Products.Add(product);
            }
            context.SaveChanges();

            /* Attachments */
            if (context.Attachments.Any())
            {
                return;
            }
            var attachments = new Attachment[]
            {
                new Attachment{ ProductID=1, Path="sciezka do pliku", Description="Instrukcja obsługi laptopa" },
                new Attachment{ ProductID=2, Path="sciezka do pliku", Description="Instrukcja obsługi smartfona" },
                new Attachment{ ProductID=3, Path="sciezka do pliku", Description="Instrukcja obsługi laptopa" },
                new Attachment{ ProductID=4, Path="sciezka do pliku", Description="Instrukcja obsługi laptopa" }
            };

            foreach (Attachment attachment in attachments)
            {
                context.Attachments.Add(attachment);
            }
            context.SaveChanges();

            /* ProductOrders */
            if (context.ProductOrders.Any())
            {
                return;
            }
            var productOrders = new ProductOrder[]
            {
                new ProductOrder{ OrderID=1, ProductID=1 },
                new ProductOrder{ OrderID=2, ProductID=2 },
                new ProductOrder{ OrderID=3, ProductID=2 },
                new ProductOrder{ OrderID=4, ProductID=3 },
                new ProductOrder{ OrderID=5, ProductID=4 }
            };

            foreach (ProductOrder productOrder in productOrders)
            {
                context.ProductOrders.Add(productOrder);
            }
            context.SaveChanges();
        }
    }
}
