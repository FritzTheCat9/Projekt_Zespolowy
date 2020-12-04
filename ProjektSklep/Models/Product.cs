using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektSklep.Models
{
    [DisplayColumn("ProductID")]
    public class Product
    {
        /* POLA */
        [Key]
        public int ProductID { get; set; }
        [Required]
        [ForeignKey("Category")]
        public int CategoryID { get; set; }
        [Required]
        [ForeignKey("Expert")]
        public int ExpertID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ProductDescription { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateAdded { get; set; }
        [Required]
        public bool Promotion { get; set; }
        [Required]
        public int VAT { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public bool Visibility { get; set; }
        [Required]
        public int SoldProducts { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("CategoryID")]
        public Category Category { get; set; }
        //[ForeignKey("ExpertID")]
        public Expert Expert { get; set; }
        public ICollection<Attachment> Attachments { get; set; }
        public ICollection<ProductOrder> ProductOrders { get; set; }

        /* METODY */
        public bool GenerateHTML()
        {
            return true;
        }
    }
}
