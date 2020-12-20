using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjektSklep.Models
{
    [DisplayColumn("CategoryID")]
    public class Category
    {
        /* POLA */
        [Key]
        [Display(Name = "KategoriaId")]
        public int CategoryID { get; set; }
        [Display(Name = "KategoriaRodzicaId")]
        public int? ParentCategoryID { get; set; }
        [Required]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Widoczność")]
        public bool Visibility { get; set; }

        /* POLA - ENTITY FRAMEWORK */
        [ForeignKey("ParentCategoryID")]
        [Display(Name = "KategoriaRodzicaId")]
        public Category Parent { get; set; }
        public ICollection<Category> Childern { get; set; }
        public ICollection<Product> Products { get; set; }

        /* METODY */
        public bool GenerateProductPriceList()
        {
            return true;
        }
    }
}
