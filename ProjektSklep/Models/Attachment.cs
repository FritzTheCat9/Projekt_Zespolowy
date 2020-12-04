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
        public int AttachmentID { get; set; }
        [Required]
        [ForeignKey("Product")]
        public int ProductID { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public string Description { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("ProductID")]
        public Product Product { get; set; }
    }
}
