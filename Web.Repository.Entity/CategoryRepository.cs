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
    public class CategoryRepository : ICategoryRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
        private const string KeyCache = "cachecategories"; 
        private const string KeyCacheView = "cachecategories_view"; 
        public void Add(Category obj)
        {
            HelperCache.RemoveCache(KeyCache);
            HelperCache.RemoveCache(KeyCacheView);
            _entities.Categories.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            HelperCache.RemoveCache(KeyCache);
            HelperCache.RemoveCache(KeyCacheView);
            var obj = Find(id);
            _entities.Categories.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(Category obj)
        {
            HelperCache.RemoveCache(KeyCache);
            HelperCache.RemoveCache(KeyCacheView);
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

       

        public Category Find(int id)
        {
            return _entities.Categories.Find(id);
        }
        public List<Category> GetAll()
        {
            var lstData = HelperCache.GetCache<List<Category>>(KeyCache);
            if (lstData == null)
            {
                lstData = _entities.Categories.ToList();
                HelperCache.AddCache(lstData, KeyCache);
            }
            return lstData;
        }
    }
}
