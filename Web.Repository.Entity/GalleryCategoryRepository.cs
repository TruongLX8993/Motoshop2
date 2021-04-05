using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Web.Core;
using Web.Model;

namespace Web.Repository.Entity
{
    public class GalleryCategoryRepository : IGalleryCategoryRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
        private const string KeyCache = "cacheGalleryCategory";
        public void Add(tbl_GalleryCategory obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.tbl_GalleryCategory.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            var obj = Find(id);
            _entities.tbl_GalleryCategory.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(tbl_GalleryCategory obj)
        {
            HelperCache.RemoveCache(KeyCache);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Model.tbl_GalleryCategory Find(int id)
        {
            return _entities.tbl_GalleryCategory.Find(id);
        }

        public List<tbl_GalleryCategory> GetAll()
        {
            var lstData = HelperCache.GetCache<List<tbl_GalleryCategory>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.tbl_GalleryCategory.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
