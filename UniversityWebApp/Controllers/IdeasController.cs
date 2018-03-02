using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UniversityDao.EF;

namespace UniversityWebApp.Controllers {
    public class IdeasController : Controller {
        private UniversityDbContext db = new UniversityDbContext();

        // GET: Ideas/Create
        public ActionResult Create() {
            ViewBag.ListCategory = db.IdeaCategories.ToList();
            return View();
        }

        // POST: Ideas/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdeaID,IdeaContent,IdeaDescription,IdeaEmotion,IdeaCategory,IdeaViewCount,CreatedDate,ModifiedDate,CreatedBy,ModifiedBy,ClosedDate,IdeaStatus")] Idea idea) {
            if (ModelState.IsValid) {
                var session = ((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]);
                if(session != null) {
                    idea.CreatedBy = session.UserID.ToString();
                }
                idea.IdeaViewCount = 0;
                idea.IdeaStatus = true;
                db.Ideas.Add(idea);
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }
            ViewBag.ListCategory = db.IdeaCategories.ToList();
            return View(idea);
        }

        protected override void Dispose(bool disposing) {
            if (disposing) {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
