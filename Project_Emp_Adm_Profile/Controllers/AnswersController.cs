using Project_Emp_Adm_Profile.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Project_Emp_Adm_Profile.Controllers
{
    public class AnswersController : Controller
    {
        // GET: Answers
        static SqlConnection connection;
        static string conStr = "server=INL402;database=ProjectDb;trusted_connection=true;";
        static SqlCommand command;
        static SqlDataReader sdr;
        static Query answers;
        // GET: Answers
        public ActionResult Index(string qId = "")
        {
            if (qId == "")
            {
                return RedirectToAction("Index", "Queries");
            }
            else
            {
                answers = new Query();
                connection = new SqlConnection(conStr);
                command = new SqlCommand()
                {
                    CommandText = "select QTitle,QDate, EName, QDesc,NumAns,QId,Category from QueryFeed q inner join Emp e on e.EId = q.EId where q.QId=@id",
                    Connection = connection
                };
                command.Parameters.AddWithValue("@id", qId);
                connection.Open();
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        answers.QTitle = sdr[0].ToString();
                        answers.QDate = sdr.GetDateTime(1);
                        answers.EName = sdr[2].ToString();
                        answers.QDesc = sdr[3].ToString();
                        answers.NumAns = (int)sdr[4];
                        answers.QId = (int)sdr[5];
                        answers.Category = sdr[6].ToString();
                    }
                }
                connection.Close();

                command.CommandText = "select EPhoto from Emp where EId=@eid";
                command.Parameters.AddWithValue("@eid", Session["EId"]);
                connection.Open();
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        answers.EPhoto = sdr[0].ToString();
                    }
                }
                connection.Close();

                command.CommandText = "select Answer,NumUpvotes,e.EName,AnsId from Answer a inner join Emp e on a.EId=e.EId where QId=@qid order by NumUpvotes Desc;";
                command.Parameters.AddWithValue("@qid", qId);
                connection.Open();
                sdr = command.ExecuteReader();
                if (sdr.HasRows)
                {
                    while (sdr.Read())
                    {
                        answers.Answers.Add(new AnswerQ { Value = sdr[0].ToString(), UpVotes = (int)sdr[1], EName = sdr[2].ToString(), AId = (int)sdr[3] });
                    }
                }
                connection.Close();

                return View(answers);
            }

        }

        [HttpPost]
        public ActionResult PostAnswer(string answer, string qid)
        {
            connection = new SqlConnection(conStr);
            command = new SqlCommand()
            {
                CommandText = "insert into Answer values (@answer, @empid, @querid, getdate(), 0)",
                Connection = connection
            };
            command.Parameters.AddWithValue("@answer", answer);
            command.Parameters.AddWithValue("@empid", Session["EId"]);
            command.Parameters.AddWithValue("@querid", qid);
            connection.Open();
            sdr = command.ExecuteReader();
            connection.Close();
            return RedirectToAction("Index", "Answers", new { qid = qid });
        }


        [HttpPost]
        public ActionResult UpVote(string aid, string qid)
        {
            try
            {
                connection = new SqlConnection(conStr);
                command = new SqlCommand()
                {
                    CommandText = "insert into UpVotes values (@aid, @eid)",
                    Connection = connection
                };
                command.Parameters.AddWithValue("@aid", aid);
                command.Parameters.AddWithValue("@eid", Session["EId"]);
                connection.Open();
                sdr = command.ExecuteReader();
                connection.Close();
                return RedirectToAction("Index", "Answers", new { qid = qid });
            }
            catch (Exception)
            {
                connection.Close();
                return RedirectToAction("Index", "Answers", new { qid = qid });
            }
        }
    }
}