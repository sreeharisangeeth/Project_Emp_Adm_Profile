using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppQueries.Models;

namespace WebAppQueries.Controllers
{
    public class NewsFeedController : Controller
    {
        static SqlConnection connection;
        static string conStr = "server=INL402;database=ProjectDb;trusted_connection=true;";
        static SqlCommand command;
        static SqlDataReader sdr;
        List<NewsFeed> listNews;
        // GET: NewsFeed
        public ActionResult Index()
        {
            listNews = new List<NewsFeed>();
            connection = new SqlConnection(conStr);
            command = new SqlCommand()
            {
                CommandText = "select NId,AName,Topic,Descrip,NDate,countlikes from NewsFeed n inner join Admin a on a.AId=n.AId order by NDate Desc;",
                Connection = connection
            };
            connection.Open();
            sdr = command.ExecuteReader();
            if (sdr.HasRows)
            {
                while (sdr.Read())
                {
                    listNews.Add(new NewsFeed { NId = (int)sdr[0], AName = sdr[1].ToString(), Topic = sdr[2].ToString(), Descrip = sdr[3].ToString(), NDate = sdr.GetDateTime(4), countlikes = (int)sdr[5] });
                }
            }
            connection.Close();

            return PartialView("News",listNews);
        }

        public  ActionResult Newsfeedlike(string nid)
        {

            try
            {
                connection = new SqlConnection(conStr);
                command = new SqlCommand()
                {
                    CommandText = "insert into newsfeedlikes values (@nid, @eid)",
                    Connection = connection
                };
                command.Parameters.AddWithValue("@nid", nid);
                command.Parameters.AddWithValue("@eid", Session["EmpId"]);

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