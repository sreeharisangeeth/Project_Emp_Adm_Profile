﻿using System.Web;
using System.Web.Mvc;

namespace Project_Emp_Adm_Profile
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}