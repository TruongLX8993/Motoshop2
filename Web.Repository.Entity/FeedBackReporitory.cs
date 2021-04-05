using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Web.Model;

namespace Web.Repository.Entity
{
    public class FeedBackReporitory : IFeedBackReporitory
    {
        readonly MotorEntities _entities = new MotorEntities();
        public void Add(Model.tbl_FeedBack obj)
        {
            _entities.tbl_FeedBack.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_FeedBack.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Model.tbl_FeedBack obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_FeedBack Find(int id)
        {
            return _entities.tbl_FeedBack.Find(id);
        }


        public IEnumerable<Model.tbl_FeedBack> GetAll()
        {
            return _entities.tbl_FeedBack.OrderByDescending(g => g.CreatedDate);
        }
    }
}
