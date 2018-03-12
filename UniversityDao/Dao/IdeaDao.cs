﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityDao.EF;

namespace UniversityDao.Dao
{
    public class IdeaDao
    {
        EF.UniversityDbContext db = null;
        public IdeaDao()
        {
            db = new EF.UniversityDbContext();
        }
        public List<Idea> GetAllIdea()
        {
            var result = db.Ideas.Where(x => x.IdeaStatus == true).ToList();
            return result;
        }
        // get Detail an idea by id
        public Idea GetIdeaById(int id)
        {
            Idea result = db.Ideas.Where(x => x.IdeaID == id && x.IdeaStatus == true).SingleOrDefault();
            return result;
        }
        // Get only the name of category idea by that id
        public string GetNameIdeaCate(int ideaCateId)
        {
            try
            {
                SqlParameter sqlParameter = new SqlParameter("@InputIdeaCategory", ideaCateId);
                string result = db.Database.SqlQuery<String>("exec sp_getIdeaCateName @InputIdeaCategory", sqlParameter).FirstOrDefault().ToString();
                return result;

            }
            catch (Exception)
            {

                throw;
            }
        }
        public string GetFileSP(int ideaId)
        {
            var rs = db.Ideas.SingleOrDefault(x => x.IdeaID == ideaId).FileSP;
            return rs;
        }
        // Thumbs Up
        public int CheckExistTU(int ideaId, int userId)
        {
            int status = 1;
            var result = (from a in db.Ideas
                          join b in db.Emotions
                          on a.IdeaID equals b.IdeaId
                          join c in db.EmotionLogs
                          on b.EmotionId equals c.EmotionId
                          where ideaId == a.IdeaID && b.EmotionName.Equals("Thumbs-Up") && c.UserId == userId
                          select new
                          {
                              UserId = c.UserId
                          }).ToList();
            if (result.Count == 0)
            {
                status = 0;
            }
            else
            {
                foreach (var item in result)
                {
                    if (item.UserId == userId)
                    {
                        RemoveThumbsLog(ideaId, userId);
                    }
                    else
                    {
                        status = 0;
                    }
                }
            }
            return status;
        }
        public int GetExistTU(int ideaId, int userId)
        {
            int status = 1;
            var result = (from a in db.Ideas
                          join b in db.Emotions
                          on a.IdeaID equals b.IdeaId
                          join c in db.EmotionLogs
                          on b.EmotionId equals c.EmotionId
                          where ideaId == a.IdeaID && b.EmotionName.Equals("Thumbs-Up") && c.UserId == userId
                          select new
                          {
                              UserId = c.UserId
                          }).ToList();
            if (result.Count == 0)
            {
                status = 0;
            }
            return status;
        }
        public int GetExistTD(int ideaId, int userId)
        {
            int status = 1;
            var result = (from a in db.Ideas
                          join b in db.Emotions
                          on a.IdeaID equals b.IdeaId
                          join c in db.EmotionLogs
                          on b.EmotionId equals c.EmotionId
                          where ideaId == a.IdeaID && b.EmotionName.Equals("Thumbs-Down") && c.UserId == userId
                          select new
                          {
                              UserId = c.UserId
                          }).ToList();
            if (result.Count == 0)
            {
                status = 0;
            }
            return status;
        }
        public int CheckExistEmoLog(int ideaId, int userId)
        {
            int status = 1;
            var result = (from a in db.Ideas
                          join b in db.Emotions
                          on a.IdeaID equals b.IdeaId
                          join c in db.EmotionLogs
                          on b.EmotionId equals c.EmotionId
                          where ideaId == a.IdeaID
                          select new
                          {
                              UserId = c.UserId
                          }).ToList();
            if (result.Count == 0)
            {
                status = 0;
            }
            else
            {
                foreach (var item in result)
                {
                    if (item.UserId == userId)
                    {
                        RemoveThumbsLog(ideaId, userId);
                        status = 2;
                    }
                    else
                    {
                        status = 0;
                    }
                }
            }
            return status;
        }
        public int CheckExistTD(int ideaId, int userId)
        {
            int status = 1;
            var result = (from a in db.Ideas
                          join b in db.Emotions
                          on a.IdeaID equals b.IdeaId
                          join c in db.EmotionLogs
                          on b.EmotionId equals c.EmotionId
                          where ideaId == a.IdeaID && b.EmotionName.Equals("Thumbs-Down") && c.UserId == userId
                          select new
                          {
                              UserId = c.UserId
                          }).ToList();
            if (result.Count == 0)
            {
                status = 0;
            }
            else
            {
                foreach (var item in result)
                {
                    if (item.UserId == userId)
                    {
                        RemoveThumbsLog(ideaId, userId);
                    }
                    else
                    {
                        status = 0;
                    }
                }
            }
            return status;
        }
        public List<EmotionLog> GetThumbsUp(int ideaId)
        {
            try
            {
                List<EmotionLog> result = (from a in db.Ideas
                                           join b in db.Emotions
                                           on a.IdeaID equals b.IdeaId
                                           join c in db.EmotionLogs
                                           on b.EmotionId equals c.EmotionId
                                           where ideaId == a.IdeaID && b.EmotionName.Equals("Thumbs-Up")
                                           select new
                                           {
                                               UserId = c.UserId,
                                               EmotiongId = c.EmotionId
                                           }).AsEnumerable().Select(x => new EmotionLog()
                                           {
                                               UserId = x.UserId,
                                               EmotionId = x.EmotiongId
                                           }).ToList();
                db.Dispose();

                return result;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public List<EmotionLog> GetThumbsLog(int ideaId, int userId)
        {
            try
            {
                List<EmotionLog> result = (from a in db.Ideas
                                           join b in db.Emotions
                                           on a.IdeaID equals b.IdeaId
                                           join c in db.EmotionLogs
                                           on b.EmotionId equals c.EmotionId
                                           where ideaId == a.IdeaID && c.UserId == userId
                                           select new
                                           {
                                               UserId = c.UserId,
                                               EmotiongId = c.EmotionId
                                           }).AsEnumerable().Select(x => new EmotionLog()
                                           {
                                               UserId = x.UserId,
                                               EmotionId = x.EmotiongId
                                           }).ToList();
                return result;
            }
            catch (Exception)
            {

                throw;
            }


        }
        public bool RemoveThumbsLog(int ideaId, int userId)
        {
            var rs = GetThumbsLog(ideaId, userId);
            var rs_emotionId = 0;
            foreach (var item in rs)
            {
                rs_emotionId = (int)item.EmotionId;
                break;
            }
            db.EmotionLogs.RemoveRange(db.EmotionLogs.Where(x => x.EmotionId == rs_emotionId && x.UserId == userId));
            db.SaveChanges();
            return true;
        }

        // Thumbs
        // Thumbs up

        public int CreateThumbsUpEmotion(int ideaId)
        {
            try
            {
                var rs = db.Emotions.FirstOrDefault(x => x.IdeaId == ideaId && x.EmotionName.Equals("Thumbs-Up"));
                if (rs != null)
                {
                    return rs.EmotionId;
                }
                else
                {
                    Emotion emotion = new Emotion();
                    emotion.EmotionName = "Thumbs-Up";
                    emotion.Description = "";
                    emotion.IdeaId = ideaId;
                    db.Emotions.Add(emotion);
                    db.SaveChanges();
                    return emotion.EmotionId;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool ThumbsUp(int ideaId, int userId)
        {
            try
            {
                var rs_emotionId = CreateThumbsUpEmotion(ideaId);
                EmotionLog emotionLog = new EmotionLog();
                emotionLog.EmotionId = rs_emotionId;
                emotionLog.UserId = userId;
                emotionLog.Emotime = DateTime.Now;
                db.EmotionLogs.Add(emotionLog);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        // Thumbs downs



        public List<EmotionLog> GetThumbsDown(int ideaId)
        {
            try
            {
                List<EmotionLog> result = (from a in db.Ideas
                                           join b in db.Emotions
                                           on a.IdeaID equals b.IdeaId
                                           join c in db.EmotionLogs
                                           on b.EmotionId equals c.EmotionId
                                           where ideaId == a.IdeaID && b.EmotionName.Equals("Thumbs-Down")
                                           select new
                                           {
                                               UserId = c.UserId,
                                               EmotiongId = c.EmotionId
                                           }).AsEnumerable().Select(x => new EmotionLog()
                                           {
                                               UserId = x.UserId,
                                               EmotionId = x.EmotiongId
                                           }).ToList();
                db.Dispose();
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public int CreateThumbsDownEmotion(int ideaId)
        {
            try
            {
                var rs = db.Emotions.FirstOrDefault(x => x.IdeaId == ideaId && x.EmotionName.Equals("Thumbs-Down"));
                if (rs != null)
                {
                    return rs.EmotionId;
                }
                else
                {
                    Emotion emotion = new Emotion();
                    emotion.EmotionName = "Thumbs-Down";
                    emotion.Description = "";
                    emotion.IdeaId = ideaId;
                    db.Emotions.Add(emotion);
                    db.SaveChanges();
                    return emotion.EmotionId;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public bool ThumbsDown(int ideaId, int userId)
        {
            try
            {
                var rs_emotionId = CreateThumbsDownEmotion(ideaId);
                EmotionLog emotionLog = new EmotionLog();
                emotionLog.EmotionId = rs_emotionId;
                emotionLog.UserId = userId;
                emotionLog.Emotime = DateTime.Now;
                db.EmotionLogs.Add(emotionLog);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }


        // End thumbs down
        // end thumbs function
        public bool InsertNewIdea(Idea model)
        {
            try
            {
                model.IdeaViewCount = 0;
                model.IdeaStatus = true;
                model.CreatedDate = DateTime.Now;
                model.ClosedDate = DateTime.Now.AddDays(30);
                db.Ideas.Add(model);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                throw;
                //return false;
            }
        }
    }
}
