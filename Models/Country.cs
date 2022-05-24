using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DB_Lab_2
{
    public partial class Country
    {
        public Country()
        {
            Brands = new HashSet<Brand>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage ="Заповніть поле")]
        [StringLength(50)]
        public string Name { get; set; } = null!;

        public virtual ICollection<Brand> Brands { get; set; }
    }
}
