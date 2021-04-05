using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class GalleryRepository : IGalleryRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
        private const string KeyCache = "Gallery";
        public void Add(Model.tbl_Gallery obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_Gallery.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_Gallery.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Model.tbl_Gallery obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_Gallery Find(int id)
        {
            return _entities.tbl_Gallery.Find(id);
        }

        public IEnumerable<Model.tbl_Gallery> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_Gallery>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_Gallery.OrderByDescending(g=>g.CreatedDate).ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }

       
    }
}
