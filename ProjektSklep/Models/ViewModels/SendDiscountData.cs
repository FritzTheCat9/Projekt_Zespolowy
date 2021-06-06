using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models.ViewModels
{
    public class SendDiscountData
    {
        public Customer Customer { get; set; }

        public DiscountCode DiscountCode { get; set; }
    }
}
