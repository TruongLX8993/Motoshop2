using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository.Entity
{
    public class NewsRepository : INewsRepository
    {
        readonly MotorEntities context = new MotorEntities();
       
        public void Add(News model)
        {
            context.News.Add(model);
            context.SaveChanges();
        }

        public IEnumerable<ListNews> ListAll(string keyWord, int status)
        {
            object[] parameters =
            {
                new SqlParameter("@MetaTitle",keyWord),
                new SqlParameter("@Status",status),
            };
            return context.Database.SqlQuery<ListNews>("Sp_News_ListAll @MetaTitle,@Status", parameters);
        }
        public IEnumerable<News> GetAll()
        {
            return context.News;
        }
       
        public void Publish(int id)
        {
            context.Database.ExecuteSqlCommand("Sp_News_Publish @ID ", new SqlParameter("@ID", id));
        }
        public void UnPublish(int id)
        {
            context.Database.ExecuteSqlCommand("Sp_News_UnPublish @ID ", new SqlParameter("@ID", id));
        }
        
          public void Edit(News model)
        {
            var obj = Find(model.ID); 
            obj.Contents = model.Contents;
            obj.MetaTitle = model.MetaTitle;
            obj.Image = model.Image;
            obj.Description = model.Description;
            obj.LinkSeo = model.LinkSeo;
            obj.CreatedBy = model.CreatedBy;
            obj.ModifiedDate = DateTime.Now;
            obj.Tags = model.Tags;
            obj.Contents = model.Contents;
            obj.CreatedDate = model.CreatedDate;
            context.Entry(obj);
            context.SaveChanges();
        
        }

        public News Find(int id)
        {
            return context.News.Find(id);
        }
        public ListNews Detail(int id)
        {
            return context.Database.SqlQuery<ListNews>("Sp_News_Detail @ID", new SqlParameter("@ID", id)).FirstOrDefault();
        }
        public bool Delete(int id)
        {
            try
            {
                var news = Find(id);
                context.News.Remove(news);
                context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
