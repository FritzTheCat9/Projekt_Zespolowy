using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models.ViewModels
{
    public class ContactFormViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
