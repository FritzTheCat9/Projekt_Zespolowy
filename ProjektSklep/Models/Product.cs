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
        [Display(Name = "ProduktId")]
        public int ProductID { get; set; }
        [Required]
        [ForeignKey("Category")]
        [Display(Name = "KategoriaId")]
        public int CategoryID { get; set; }
        [Required]
        [ForeignKey("Expert")]
        [Display(Name = "EkspertId")]
        public int ExpertID { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Opis")]
        public string ProductDescription { get; set; }
        [Required]
        [Display(Name = "Obraz")]
        public string Image { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data dodania")]
        public DateTime DateAdded { get; set; }
        [Required]
        [Display(Name = "Promocja")]
        public bool Promotion { get; set; }
        [Required]
        [Display(Name = "VAT")]
        public int VAT { get; set; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Display(Name = "Cena")]
        public decimal Price { get; set; }
        [Required]
        [Display(Name = "Ilość")]
        public int Amount { get; set; }
        [Required]
        [Display(Name = "Widoczność")]
        public bool Visibility { get; set; }
        [Required]
        [Display(Name = "Sprzedano")]
        public int SoldProducts { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        //[ForeignKey("CategoryID")]
        [Display(Name = "KategoriaId")]
        public Category Category { get; set; }
        //[ForeignKey("ExpertID")]
        [Display(Name = "EkspertId")]
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
