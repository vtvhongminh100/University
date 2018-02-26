using ModelPr.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDao.EF;

namespace UniversityDao.Dao
{
    public class IdeaCategoryDao
    {
        EF.UniversityDbContext db = null;
        public IdeaCategoryDao()
        {
            db = new EF.UniversityDbContext();
        }
        public List<IdeaCategoryView> GetAllIdeaCategory(int ID)
        {
            var result = (from a in db.CategoryGroupIdeas
                          join b in db.IdeaCategories
                          on a.CategoryGroupIdeaID equals b.GroupCateIdea
                          where b.GroupCateIdea == ID
                          select new IdeaCategoryView()
                          {
                              IdeaCategoryID = b.IdeaCategoryID,
                              CategoryName = b.CategoryName,
                              CategoryDescription = b.CategoryDescription,
                              CreatedBy = b.CreatedBy,
                              CreatedDate = (DateTime)b.CreatedDate,
                              ModifiedDate = (DateTime)b.ModifiedDate,
                              GroupCateIdea = (int)b.GroupCateIdea,
                              IdeaCateViewC = (int)b.IdeaCateViewC,
                              IdeaCateStatus = (bool)b.IdeaCateStatus,
                              ModifiedBy = b.ModifiedBy,
                          }).ToList();

            return result;
        }
        public bool InsertIdeaCategory(IdeaCategory model)
        {
            try
            {
                model.IdeaCateViewC = 0;
                model.CreatedDate = DateTime.Now;
                model.ModifiedDate = DateTime.Now;
                db.IdeaCategories.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateIdeaCategory(IdeaCategory model)
        {
            try
            {
                var idc = db.IdeaCategories.Find(model.IdeaCategoryID);
                idc.ModifiedDate = DateTime.Now;
                idc.IdeaCateStatus = model.IdeaCateStatus;
                idc.ModifiedBy = model.ModifiedBy;
                idc.GroupCateIdea = model.GroupCateIdea;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IdeaCategory GetIdeaCateByID(int ID)
        {
            var result = db.IdeaCategories.SingleOrDefault(x => x.IdeaCategoryID == ID);
            return result;

        }
        public bool DeleteIdeaCategory(int id)
        {
            try
            {
                var idc = db.IdeaCategories.Find(id);
                db.IdeaCategories.Remove(idc);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
