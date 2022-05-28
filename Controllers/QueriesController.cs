using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using DB_Lab_2.Models;

namespace DB_Lab_2.Controllers
{
    public class QueriesController : Controller
    {










        private const string CONN_STR = "Server= DESKTOP-MF2OKS0; Database=DBLab2; Trusted_Connection=True;MultipleActiveResultSets=true";

        private const string S1_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\S1.sql";
        private const string S2_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\S2.sql";
        private const string S3_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\S3.sql";
        private const string S4_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\S4.sql";
        private const string S5_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\S5.sql";
        private const string S6_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\S6.sql";

        private const string A1_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\A1.sql";
        private const string A2_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\A2.sql";
        private const string A3_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\A3.sql";

        private const string T1_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\T1.sql";
        private const string T2_PATH = @"C:\Users\lenovo\Desktop\projects\DB_Lab_2\Queries\T2.sql";

        private const string ERR_AVG = "Неможливо обрахувати середню ціну, оскільки продукти відсутні.";
        private const string ERR_CUST = "Покупці, що задовольняють дану умову, відсутні.";
        private const string ERR_PROD = "Програмні продукти, що задовольняють дану умову, відсутні.";
        private const string ERR_DEV = "Розробники, що задовольняють дану умову, відсутні.";
        private const string ERR_COUNTRY = "Країни, що задовольняють дану умову, відсутні.";

        private readonly DBLab2Context _context;

        public QueriesController(DBLab2Context context)
        {
            _context = context;
        }

        public IActionResult Index(int errorCode)
        {
            var customers = _context.Persons.Select(c => c.Name).Distinct().ToList();
            if (errorCode == 1)
            {
                ViewBag.ErrorFlag = 1;
                ViewBag.PriceError = "Введіть коректну вартість";
            }
            if (errorCode == 2)
            {
                ViewBag.ErrorFlag = 2;
                ViewBag.ProdNameError = "Поле необхідно заповнити";
            }

            var empty = new SelectList(new List<string> { "--Пусто--" });
            var anyCusts = _context.Persons.Any();
            var anyDevs = _context.Brands.Any();

            ViewBag.DevIds = anyDevs ? new SelectList(_context.Brands, "Id", "Id") : empty;
            ViewBag.DevNames = anyDevs ? new SelectList(_context.Brands, "Name", "Name") : empty;
            ViewBag.CustNames = anyCusts ? new SelectList(customers) : empty;
            ViewBag.CustEmails = anyCusts ? new SelectList(_context.Persons, "Email", "Email") : empty;
            ViewBag.CustSurnames = anyCusts ? new SelectList(_context.Persons, "Surname", "Surname") : empty;
            ViewBag.Countries = _context.Countries.Any() ? new SelectList(_context.Countries, "Name", "Name") : empty;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S1_PATH);
            query = query.Replace("P", "N\'" + queryModel.BrandName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S1";

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        queryModel.AvgPrice = Convert.ToDecimal(result);
                    }
                    else
                    {
                        queryModel.ErrorFlag = 1;
                        queryModel.Error = ERR_AVG;
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S2_PATH);
            query = query.Replace("X", "N\'" + queryModel.BrandName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S2";
            queryModel.PersonsNames = new List<string>();
            queryModel.PersonsSurnames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PersonsNames.Add(reader.GetString(0));
                            queryModel.PersonsSurnames.Add(reader.GetString(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_CUST;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S3_PATH);
            query = query.Replace("K", "N\'" + queryModel.CountryName + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S3";
            queryModel.CarNames = new List<string>();
            queryModel.CarPrices = new List<decimal>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CarNames.Add(reader.GetString(0));
                            queryModel.CarPrices.Add(reader.GetDecimal(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_PROD;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery4(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(S4_PATH);
            query = query.Replace("X", "N\'" + queryModel.PersonName + "\'");
            query = query.Replace("Y", "N\'" + queryModel.PersonSurname + "\'");
            query = query.Replace("Z", "N\'" + queryModel.PersonEmail + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');

            queryModel.QueryId = "S4";
            queryModel.BrandNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.BrandNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_DEV;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery5(Query queryModel)
        {
            
            if (ModelState.IsValid)
            {
                string query = System.IO.File.ReadAllText(S5_PATH);
                query = query.Replace("P", queryModel.Price.ToString());
                query = query.Replace("\r\n", " ");
                query = query.Replace('\t', ' ');

                queryModel.QueryId = "S5";
                queryModel.BrandNames = new List<string>();

                using (var connection = new SqlConnection(CONN_STR))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.BrandNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.Error = ERR_DEV;
                            }
                        }
                    }
                    connection.Close();
                }
                return RedirectToAction("Result", queryModel);
            }
            return RedirectToAction("Index", new { errorCode = 1 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SimpleQuery6(Query queryModel)
        {
            if (ModelState.IsValid)
            {
                string query = System.IO.File.ReadAllText(S6_PATH);
                query = query.Replace("X", "N\'" + queryModel.CarName + "\'");
                query = query.Replace("\r\n", " ");
                query = query.Replace('\t', ' ');
                queryModel.QueryId = "S6";
                queryModel.BrandNames = new List<string>();

                using (var connection = new SqlConnection(CONN_STR))
                {
                    connection.Open();
                    using (var command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        using (var reader = command.ExecuteReader())
                        {
                            int flag = 0;
                            while (reader.Read())
                            {
                                queryModel.BrandNames.Add(reader.GetString(0));
                                flag++;
                            }

                            if (flag == 0)
                            {
                                queryModel.ErrorFlag = 1;
                                queryModel.Error = ERR_DEV;
                            }
                        }
                    }
                    connection.Close();
                }
                return RedirectToAction("Result", queryModel);
            }

            return RedirectToAction("Index", new { errorCode = 2 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery1(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A1_PATH);
            query = query.Replace("K", queryModel.BrandId.ToString());
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "A1";
            queryModel.CountryNames = new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.CountryNames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_COUNTRY;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdvancedQuery2(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A2_PATH);
            query = query.Replace("Y", "N\'" + queryModel.PersonEmail.ToString() + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "A2";
            queryModel.PersonsSurnames= new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PersonsSurnames.Add(reader.GetString(0));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_CUST;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult AdvancedQuery3(Query queryModel)
        {
            string query = System.IO.File.ReadAllText(A3_PATH);
            query = query.Replace("Y", "N\'" + queryModel.PersonName.ToString() + "\'");
            query = query.Replace("\r\n", " ");
            query = query.Replace('\t', ' ');
            queryModel.QueryId = "A3";
            queryModel.PersonsSurnames = new List<string>();
            queryModel.PersonsEmails= new List<string>();

            using (var connection = new SqlConnection(CONN_STR))
            {
                connection.Open();
                using (var command = new SqlCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                    using (var reader = command.ExecuteReader())
                    {
                        int flag = 0;
                        while (reader.Read())
                        {
                            queryModel.PersonsSurnames.Add(reader.GetString(0));
                            queryModel.PersonsEmails.Add(reader.GetString(1));
                            flag++;
                        }

                        if (flag == 0)
                        {
                            queryModel.ErrorFlag = 1;
                            queryModel.Error = ERR_CUST;
                        }
                    }
                }
                connection.Close();
            }
            return RedirectToAction("Result", queryModel);
        }

        public IActionResult TeacherQuery1(Query queryModel)
        {
            throw new NotImplementedException();
        }

        public IActionResult TeacherQuery2(Query queryModel)
        {
            throw new NotImplementedException();
        }

        public IActionResult Result(Query queryResult)
        {
            return View(queryResult);
        }











    }
}
