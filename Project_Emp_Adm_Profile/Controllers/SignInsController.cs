using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Security;
using Project_Emp_Adm_Profile.Models;

namespace Project_Emp_Adm_Profile.Controllers
{
    public class SignInsController : Controller
    {
        private ProjectDbEntities db = new ProjectDbEntities();

        public ActionResult signOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();

            //HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            //cookie1.Expires = DateTime.Now.AddYears(-1);
            //Response.Cookies.Add(cookie1);

            //SessionStateSection sessionStateSection = (SessionStateSection)WebConfigurationManager.GetSection("system.web/sessionState");
            //HttpCookie cookie2 = new HttpCookie(sessionStateSection.CookieName, "");
            //cookie2.Expires = DateTime.Now.AddYears(-1);
            //Response.Cookies.Add(cookie2);

            return RedirectToAction("SignIn", "SignIns");
        }
        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoginPage()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SignIn(SignIn sign)
        {
            if (!ModelState.IsValid)
                {
                    return View(sign);
                }
                else
                {
                    if (sign.UserName.Contains("admin"))
                    {
                        Admin admin = db.Admins.SingleOrDefault(name => name.AName == sign.UserName);
                        if ((admin != null) && (admin.AName == sign.UserName) && (admin.APassword == sign.Password))
                        {

                            //SignIn signIn = db.SignIns.SingleOrDefault(UName => UName.UserName == sign.UserName);
                            // if ((admin.AName == sign.UserName) && (admin.APassword == sign.Password))
                            Session["AId"] = admin.AId;
                            Session["AdminObj"] = admin;
                            Session["AdminId"] = Guid.NewGuid().ToString();
                            Session["Name"] = sign.UserName;
                            //return RedirectToAction("Index", "Admins");
                            return RedirectToAction("Index", "Queries");

                        }
                        else
                        {
                            ModelState.AddModelError("", "Sign In Failed!! No such " + sign.UserName + " Or Wrong Password");
                        }
                    }
                    else
                    {
                        Emp emp = db.Emps.SingleOrDefault(name => name.EName == sign.UserName);
                        if ((emp != null))
                        {
                            if ((emp.EName == sign.UserName) && (emp.EPassword == sign.Password))
                            {
                                Session["EId"] = emp.EId;
                                Session["EmployeeObj"] = emp;
                                Session["EmpId"] = Guid.NewGuid().ToString();
                                Session["Name"] = sign.UserName;
                                return RedirectToAction("Index", "Queries");
                                //return RedirectToAction("Dashboard");
                                //return RedirectToAction("Details", "Emps", new { id = emp.EId });
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "Sign In Failed!! No such " + sign.UserName + " Or Wrong Password");
                        }

                    }
                }
            
            return View();
        }
    }
}
