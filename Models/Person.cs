using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DB_Lab_2
{
    public partial class Person
    {
        private const string ERR_REQ = "Заповніть поле";
        private const string RGX_EMAIL = @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z";
        
        public Person()
        {
            Purchases = new HashSet<Purchase>();
        }

        public int Id { get; set; }

        [Required(ErrorMessage =ERR_REQ)]
        [StringLength(50)]
        [Display(Name = "Ім\'я")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = ERR_REQ)]
        [StringLength(50)]
        [Display(Name = "Прізвище")]
        public string Surname { get; set; } = null!;

        [Required(ErrorMessage = ERR_REQ)]
        [StringLength(50)]
        [Display(Name = "Електронна пошта")]
        [RegularExpression(RGX_EMAIL,ErrorMessage ="Введіть коректну ел. пошту")]
        public string Email { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; }
    }
}
