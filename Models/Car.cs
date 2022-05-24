using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DB_Lab_2
{
    public partial class Car
    {
        private const string ERR_REQ = "Поле необхідно заповнити";
        public Car()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int Id { get; set; }
        [Display(Name = "Марка")]
        public int BrandId { get; set; }

        [Required(ErrorMessage = ERR_REQ)]
        [StringLength(50)]
        [Display(Name = "Назва")]
        public string Name { get; set; } = null!;


        [Required(ErrorMessage = ERR_REQ)]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "Введіть коректну вартість")]
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = ERR_REQ)]
        [Display(Name = "Опис")]
        public string Description { get; set; } = null!;

        [Display(Name = "Марка")]
        public virtual Brand Brand { get; set; } = null!;
        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
