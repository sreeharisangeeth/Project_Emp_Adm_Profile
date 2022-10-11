using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAppQueries.Models
{
    public class NewsFeed1
    {
        public int NId { get; set; }
        public string AName { get; set; }
        public string Topic { get; set; }
        public string Descrip { get; set; }
        public DateTime NDate { get; set; }
        public int countlikes { get; set; }
    }
}