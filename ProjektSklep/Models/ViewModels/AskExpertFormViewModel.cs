using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models.ViewModels
{
    public class AskExpertFormViewModel
    {
        [Key]
        public int ExpertId { get; set; }
        public int ProductId { get; set; }

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
