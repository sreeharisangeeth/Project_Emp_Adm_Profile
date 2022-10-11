using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_Emp_Adm_Profile.Filter;
using Project_Emp_Adm_Profile.Models;

namespace Project_Emp_Adm_Profile.Controllers
{
    public class AdminsController : Controller
    {
        private ProjectDbEntities db = new ProjectDbEntities();

        public ActionResult signOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("SignIn", "SignIns");
        }

        // GET: Admins
        [AdminAuth]
        public ActionResult Index()
        {
            return View(db.Admins.ToList());
        }

        // GET: Admins/Details/5
        [AdminAuth]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Admin admin = db.Admins.Find(id);
            if (admin == null)
            {
                return HttpNotFound();
            }
            return View(admin);
        }

        // GET: Admins/Create
        [AdminAuth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AId,AName,APassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Admins.Add(admin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(admin);
        }

        // GET: Admins/Edit/5
        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int OwnAID = Convert.ToInt32(Session["AID"]);
            if (Session["AId"] != null)
            {
                if (id == OwnAID)
                {
                    Admin admin = db.Admins.Find(id);
                    if (admin == null)
                    {
                        return HttpNotFound();
                    }
                    return View(admin);
                }
                else
                {
                    ViewBag.EditStatus = "Can't Edit Other Admin Details!!";
                    return RedirectToAction("Index") ;
                }
            }
            ViewBag.EditStatus = "Can't Edit Other Admin Details!!";

            return View(db.Admins.Find(OwnAID));
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AId,AName,APassword")] Admin admin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(admin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(admin);
        }

        // GET: Admins/Delete/5
        [AdminAuth]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int OwnAID = Convert.ToInt32(Session["AID"]);
            if (Session["AId"] != null)
            {
                if (id == OwnAID)
                {
                    Admin admin = db.Admins.Find(id);
                    if (admin == null)
                    {
                        return HttpNotFound();
                    }
                    return View(admin);
                }
                else
                {
                    ViewBag.DeleteStatus = "Can't delete Other Admin Details!!";
                    return RedirectToAction("Index");
                }
            }
            ViewBag.DeleteStatus = "Can't delete Other Admin Details!!";

            return View(db.Admins.Find(OwnAID));
        }
        [AdminAuth]
        // POST: Admins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Admin admin = db.Admins.Find(id);
            db.Admins.Remove(admin);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
