using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityDao.Dao;
using UniversityDao.EF;

namespace UniversityWebApp.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Test()
        {
            CommentDao dao = new CommentDao();
            Comment c = new Comment();
            c.CommentID = 1;
            c.IdeaID = 1;
            c.Description = "lorem ipsum";
            dao.InsertComment(c);
            return View();
        }
    }
}