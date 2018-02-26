using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityDao.Dao;
using UniversityDao.EF;

namespace UniversityWebApp.Areas.QAManager.Controllers
{
    public class IdeaCategoryController : BaseController
    {
        // GET: QAManager/CategoryIdea
       
        public ActionResult Index(int ID)
        {
            var result = new IdeaCategoryDao().GetAllIdeaCategory(ID);
            TempData["grID"] = ID;
            return View(result);
        }
 
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(IdeaCategory model)
        {
            model.GroupCateIdea = (int)TempData["grID"];
            model.CreatedBy =((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]).Username;
            var result = new IdeaCategoryDao().InsertIdeaCategory(model);
            return RedirectToAction("Index/" + (int)TempData["grID"]);
        }
        [HttpGet]
        public ActionResult Update(int id, int grID)
        {
            ViewBag.GroupCateIdea = new SelectList(new CateGrIdeaDao().GetAllGrCateIdea(), "CategoryGroupIdeaID", "CategoryGroupName", grID);
            var result = new IdeaCategoryDao().GetIdeaCateByID(id);
            return View(result);
        }
        [HttpPost]
        public ActionResult Update(IdeaCategory model)
        {
            model.ModifiedBy = ((UserLogin)Session[ModelPr.CommonClass.CommonCls.User_session]).Username;
            var result = new IdeaCategoryDao().UpdateIdeaCategory(model);
            return RedirectToAction("Index/" + (int)TempData["grID"]);
        }
        public ActionResult Delete(int id)
        {
            new IdeaCategoryDao().DeleteIdeaCategory(id);
            return RedirectToAction("Index/" + (int)TempData["grID"]);
        }
    }
}