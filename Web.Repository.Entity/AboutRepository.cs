using System;
using System.Collections.Generic;
using Web.Model;

namespace Web.Repository.Entity
{
    public class AboutRepository : IAboutRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
       
        public void Add(About obj)
        {
            _entities.Abouts.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.Abouts.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(About model)
        {
            var obj = Find(model.ID);
            obj.Contents = model.Contents;
            obj.MetaTitle = model.MetaTitle;
            obj.Tags = model.Tags;
            obj.Contents = model.Contents;
            obj.Createddate = model.Createddate;
            _entities.Entry(obj);
            _entities.SaveChanges();
        }
        public About Find(int id)
        {
            return _entities.Abouts.Find(id);
        }

        public IEnumerable<About> GetAll()
        {
            var lstData = _entities.Abouts;
            return lstData;
        }
    }
}
