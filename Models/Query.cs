using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DB_Lab_2.Models
{
    public class Query
    {
        public string QueryId { get; set; }

        public string Error { get; set; }

        public int ErrorFlag { get; set; }



        public string BrandName { get; set; }

        public string CountryName { get; set; }

        public decimal AvgPrice { get; set; }

        public List<string> PersonsNames { get; set; }

        public List<string> PersonsSurnames { get; set; }

        public List<string> PersonsEmails { get; set; }

        public List<string> CarNames { get; set; }

        public List<decimal> CarPrices { get; set; }

        public string PersonName { get; set; }

        public string PersonSurname { get; set; }

        public string PersonEmail { get; set; }

        public List<string> BrandNames { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        [RegularExpression(@"^[0-9]+(\.[0-9]{1,2})?$", ErrorMessage = "Введіть коректну вартість")]
        [Display(Name = "Вартість В")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Заповніть поле")]
        public string CarName { get; set; }

        public int BrandId { get; set; }

        public List<string> CountryNames { get; set; }




    }
}
