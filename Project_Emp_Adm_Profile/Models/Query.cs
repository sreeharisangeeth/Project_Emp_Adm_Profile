using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project_Emp_Adm_Profile.Models
{
    public class AnswerQ
    {
        public int UpVotes { get; set; }
        public string Value { get; set; }

        public string EName { get; set; }

        public int AId { get; set; }
    }
    public class Query
    {
        public int QId { get; set; }
        public string QTitle { get; set; }

        public string QDesc { get; set; }

        public DateTime QDate { get; set; }

        public int NumAns { get; set; }

        public string EName { get; set; }

        public string EPhoto { get; set; }

        public string Category { get; set; }


        public List<AnswerQ> Answers = new List<AnswerQ>();


    }
}