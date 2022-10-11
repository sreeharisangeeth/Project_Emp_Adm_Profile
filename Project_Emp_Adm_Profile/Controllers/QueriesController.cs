using Project_Emp_Adm_Profile.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Emp_Adm_Profile.Controllers
{
    public class QueriesController : Controller
    {
        static SqlConnection connection;
        static string conStr = "server=INL402;database=ProjectDb;trusted_connection=true;";
        static SqlCommand command;
        static SqlDataReader sdr;
        static List<QueriesFeed> listQueries;
        // GET: Queries
        public ActionResult Index(string category, string searchText)
        {
            listQueries = new List<QueriesFeed>();
            connection = new SqlConnection(conStr);
            if (category != "All" && searchText != null)
            {
                command = new SqlCommand()
                {
                    CommandText = "select QTitle, Category, QDate, EName, EDesig, EPhoto, QId from QueryFeed q inner join Emp e on e.EId = q.EId where Category=@category and (QTitle like '%'+@term+'%' or QDesc like '%'+@term+'%') order by QDate desc",
                    Connection = connection
                };
                command.Parameters.AddWithValue("@category", category);
                command.Parameters.AddWithValue("@term", searchText);
            }
            else if (category == "All")
            {
                command = new SqlCommand()
                {
                    CommandText = "select QTitle, Category, QDate, EName, EDesig, EPhoto, QId from QueryFeed q inner join Emp e on e.EId = q.EId where QTitle like '%' + @term + '%' or QDesc like '%' + @term + '%' order by QDate desc",
                    Connection = connection
                };
                command.Parameters.AddWithValue("@term", searchText);
            }
            else
            {
                command = new SqlCommand()
                {
                    CommandText = "select QTitle, Category, QDate, EName, EDesig, EPhoto,QId from QueryFeed q inner join Emp e on e.EId = q.EId order by QDate desc",
                    Connection = connection
                };
            }
            connection.Open();
            sdr = command.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    listQueries.Add(new QueriesFeed { QTitle = sdr[0].ToString(), Category = sdr[1].ToString(), QDate = sdr.GetDateTime(2), EName = sdr[3].ToString(), EDesig = sdr[4].ToString(), EPhoto = sdr[5].ToString(), QId = (int)sdr[6] });
                }
            }
            connection.Close();
            return View(listQueries);
        }

        [HttpPost]
        public ActionResult PostQuery(string title, string description, string newCategory)
        {
            try
            {
                command.CommandText = "insert into QueryFeed values (@title, @desc, @category, getdate(), @eid, 0)";
                command.Parameters.AddWithValue("@title", title);
                command.Parameters.AddWithValue("@desc", description);
                command.Parameters.AddWithValue("@category", newCategory);
                command.Parameters.AddWithValue("@eid", Session["EId"]);
                connection.Open();
                sdr = command.ExecuteReader();
                connection.Close();
                return RedirectToAction("Index", "Queries");
            }
            catch (Exception)
            {
                connection.Close();
                return RedirectToAction("Index", "Queries");
            }
        }
    }
}