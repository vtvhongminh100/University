using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.SessionState;
using UniversityDao.Dao;
using UniversityDao.EF;

namespace UniversityWebApp.Areas.Student.Controllers
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]

    //[SessionState(SessionStateBehavior.Disabled)]
    public class IdeaController : Controller
    {
        // GET: Student/Idea
        public ActionResult Index()
        {
            var result = new IdeaDao().GetAllIdea();
            GetNameIdeaCate(1);
            return View(result);
        }
        
        [HttpGet]
        public ActionResult PostIdea()
        {
            ViewBag.GroupCateIdea = new SelectList(new CateGrIdeaDao().GetAllGrCateIdea(), "CategoryGroupIdeaID", "CategoryGroupName");
            return View();
        }
        public JsonResult GetCateIdeaByJs(int grCateId)
        {
            var result =  new IdeaCategoryDao().GetIdeaCateByGr(grCateId);
            return Json(result,JsonRequestBehavior.AllowGet);
        }
        public void GetNameIdeaCate(int ideaCateId)
        {
            ViewBag.IdeaCateName = new IdeaDao().GetNameIdeaCate(1);
        }
        public void ThumbsUp(int ideaId)
        {
           
            var userId = ((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]).UserID;
            new IdeaDao().CheckExistTD(ideaId, userId);
            var rs_exEmotion = new IdeaDao().CheckExistTU(ideaId, userId);
            
            if (rs_exEmotion == 0 )
            {
                new IdeaDao().ThumbsUp(ideaId, userId);
            }
        }

        public JsonResult GetThumbsUpCountUC(int ideaId)
        {
            int ThumbsCount = new IdeaDao().GetThumbsUp(ideaId).Count();
            return Json(ThumbsCount, JsonRequestBehavior.AllowGet);
        }

        public void ThumbsDown(int ideaId)
        {

            var userId = ((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]).UserID;
            new IdeaDao().CheckExistTU(ideaId, userId);
            var rs_exEmotion = new IdeaDao().CheckExistTD(ideaId, userId);
            if (rs_exEmotion == 0 )
            {
                new IdeaDao().ThumbsDown(ideaId, userId);
            }
        }
        public JsonResult CheckExistTU(int ideaId)
        {
            var userId = ((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]).UserID;
            var rs_exEmotion = new IdeaDao().GetExistTU(ideaId, userId);
            return Json(rs_exEmotion, JsonRequestBehavior.AllowGet);

        }
        public JsonResult CheckExistTD(int ideaId)
        {
            var userId = ((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]).UserID;
            var rs_exEmotion = new IdeaDao().GetExistTD(ideaId, userId);
            return Json(rs_exEmotion, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetThumbsDownCountUC(int ideaId)
        {
            int ThumbsCount = new IdeaDao().GetThumbsDown(ideaId).Count();
            return Json(ThumbsCount, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDtIdById(int id)
        {

            string path = new IdeaDao().GetFileSP(id);
            if (path != null)
            {
                string fileName = path.Substring(path.LastIndexOf('/') + 1);
                ViewBag.FileNameSP = fileName;
            }
            ViewBag.ViewIdea = new IdeaDao().GetIdeaById(id);
            return View();
        }
        public ActionResult DownloadFile(int ideaId)
        {
            string path = new IdeaDao().GetFileSP(ideaId);
            string fileName = path.Substring(path.LastIndexOf('/') + 1);

            string basePath = AppDomain.CurrentDomain.BaseDirectory + "Data/StudentData/";
            byte[] fileBytes = System.IO.File.ReadAllBytes(basePath + fileName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        [HttpPost]
        public ActionResult PostIdea(Idea model, HttpPostedFileBase file)
        {


            if (file != null)
            {
                string fileName = Path.GetFileName(file.FileName);
                string pathToSave = Server.MapPath("~/Data/StudentData/");
                string path = Path.Combine(pathToSave, fileName);
                file.SaveAs(path);
                model.FileSP = "~/Data/StudentData/"+fileName;

            }
            new IdeaDao().InsertNewIdea(model);

            return RedirectToAction("PostIdea");
        }
    }
}