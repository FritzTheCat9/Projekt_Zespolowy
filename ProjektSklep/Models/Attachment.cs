using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektSklep.Models
{
    [DisplayColumn("AttachmentID")]
    public class Attachment
    {
        /* POLA */
        [Key]
        [Display(Name = "ZałącznikId")]
        public int AttachmentID { get; set; }
        [Required]
        [ForeignKey("Product")]
        [Display(Name = "ProduktId")]
        public int ProductID { get; set; }
        [Required]
        [Display(Name = "Ścieżka")]
        public string Path { get; set; }
        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("ProductID")]
        [Display(Name = "ProduktId")]
        public Product Product { get; set; }
    }
}
