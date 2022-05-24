using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DB_Lab_2
{
    public partial class Brand
    {
        public Brand()
        {
            Cars = new HashSet<Car>();
        }

        public int Id { get; set; }

        [Display(Name ="Країна")]
        public int CountryId { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [StringLength(50)]
        [Display(Name = "Прізвище")]
        public string Name { get; set; } = null!;

        [Display(Name = "Країна")]
        public virtual Country Country { get; set; } = null!;
        public virtual ICollection<Car> Cars { get; set; }
    }
}
