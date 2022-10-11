using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Project_Emp_Adm_Profile.Filter;
using Project_Emp_Adm_Profile.Models;

namespace Project_Emp_Adm_Profile.Controllers
{
    public class EmpsController : Controller
    {
        private ProjectDbEntities db = new ProjectDbEntities();

        public ActionResult signOut()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("SignIn", "SignIns");
        }

        // GET: Emps
        [AdminAuth]
        public ActionResult Index()
        {
            return View(db.Emps.ToList());
        }

        // GET: Emps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Session["EId"] != null)
            {
                int OwnEID = Convert.ToInt32(Session["EID"]);
                if (id == OwnEID)
                {
                    Emp emp = db.Emps.Find(id);
                    if (emp == null)
                    {
                        return HttpNotFound();
                    }
                    return PartialView("PartialViewEmpDetails", emp);
                }
                else
                {
                    ViewBag.DeailsStatus = "Can't See Other Employee Details!!";
                    return RedirectToAction("Index");
                }
            }
            else if (Session["AId"]!= null)
            {
                return View(db.Emps.Find(id));

            }
            return HttpNotFound();
        }

        // GET: Emps/Create
        [AdminAuth]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Emps/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [AdminAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EId,EName,EDesig,EPhoto,EPassword")] Emp emp,HttpPostedFileBase EPhoto)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    if (EPhoto != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Upload"), Path.GetFileName(EPhoto.FileName));
                        EPhoto.SaveAs(path);
                        emp.EPhoto = "~/Upload/" + Path.GetFileName(EPhoto.FileName);
                        ViewBag.FileStatus = "File uploaded successfully.";
                    }
                    else
                    {
                        emp.EPhoto = "Not Uploaded";

                    }
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading."; ;
                }
                db.Emps.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(emp);
        }
        [EmpAuth]
        // GET: Emps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Emp emp = db.Emps.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        // POST: Emps/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [EmpAuth]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EId,EName,EDesig,EPhoto,EPassword")] Emp emp, HttpPostedFileBase EPhoto )
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (EPhoto != null)
                    {
                        string path = Path.Combine(Server.MapPath("~/Upload"), Path.GetFileName(EPhoto.FileName));
                        EPhoto.SaveAs(path);
                        emp.EPhoto = "~/Upload/" + Path.GetFileName(EPhoto.FileName);
                        ViewBag.FileStatus = "File uploaded successfully.";
                    }
                    else
                    {
                        emp.EPhoto = "Not Uploaded";

                    }
                }
                catch (Exception)
                {
                    ViewBag.FileStatus = "Error while file uploading."; ;
                }
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Queries");
            }
            return View(emp);
        }

        // GET: Emps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            int OwnEID = Convert.ToInt32(Session["EID"]);
            if (Session["EId"] != null)
            {
                if (id == OwnEID)
                {
                    Emp emp = db.Emps.Find(id);
                    if (emp == null)
                    {
                        return HttpNotFound();
                    }
                    return View(emp);
                }
                else
                {
                    ViewBag.DetailsStatus = "Can't delete Other Employee Details!!";
                    return RedirectToAction("Index");
                }
            }
            else if (Session["AId"] != null)
            {
                return View(db.Emps.Find(id));
            }
            return HttpNotFound();
        }

        // POST: Emps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Emp emp = db.Emps.Find(id);
            db.Emps.Remove(emp);
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
