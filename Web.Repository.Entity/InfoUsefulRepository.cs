using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Web.Model;

namespace Web.Repository.Entity
{
    public class InfoUsefulRepository : IInfoUsefulRepository
    {
        private readonly MotorEntities context = new MotorEntities();
        public void Add(InfoUseful model)
        {
            context.InfoUsefuls.Add(model);
            context.SaveChanges();
        }
        public IEnumerable<InfoUseful> GetAll()
        {
            return context.InfoUsefuls;
        }
        public void Delete(int  id)
        {
            var obj = Find(id);
            context.InfoUsefuls.Remove(obj);
            context.SaveChanges();
        }
        public void Edit(InfoUseful model)
        {
            var obj = Find(model.ID);
            obj.Contents = model.Contents;
            obj.MetaTitle = model.MetaTitle;
            obj.Description = model.Description;
            obj.LinkSeo = model.LinkSeo;
            obj.CreatedBy = model.CreatedBy;
            obj.ModifiedDate = DateTime.Now;
            obj.Contents = model.Contents;
            obj.CreatedDate = model.CreatedDate;
            context.Entry(obj);
            context.SaveChanges();
        }
       
        public InfoUseful Find(int id)
        {
            return context.InfoUsefuls.Find(id);
        }
    }
}
