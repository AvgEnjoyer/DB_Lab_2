using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace DB_Lab_2
{
    public partial class Purchase
    {

        public int Id { get; set; }
        
        public int PersonId { get; set; }
        public int CarId { get; set; }

        [Display(Name = "Дата оформлення покупки")]
        public DateTime Date { get; set; }

        public virtual Car Car { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
    }
}
