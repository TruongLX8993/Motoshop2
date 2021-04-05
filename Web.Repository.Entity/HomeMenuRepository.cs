using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Web.Model;

namespace Web.Repository.Entity
{
    public class HomeMenuRepository : IHomeMenuRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
        private const string KeyCache = "HomeMenu";
        public void Add(HomeMenu model)
        {
            object[] parameters =
             {
                new SqlParameter("@Name",model.Name ),
                new SqlParameter("@LinkSeo",(object)model.LinkSeo?? DBNull.Value),
                new SqlParameter("@Icon",(object) model.Icon?? DBNull.Value),
                new SqlParameter("@ParentId", model.ParentId),
                new SqlParameter("@Ordering", model.Ordering)
            };
            _entities.Database.ExecuteSqlCommand("Sp_HomeMenu_Insert @Name,@LinkSeo,@Icon,@ParentId,@Ordering", parameters);
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.HomeMenus.Remove(obj);
            _entities.SaveChanges();
        }

        public void Edit(HomeMenu model)
        {
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Name", model.Name),
                new SqlParameter("@LinkSeo",(object)model.LinkSeo?? DBNull.Value),
                new SqlParameter("@Icon",(object) model.Icon?? DBNull.Value),
                new SqlParameter("@ParentId", model.ParentId),
                new SqlParameter("@Ordering", model.Ordering)
            };
            _entities.Database.ExecuteSqlCommand("Sp_HomeMenu_Update @ID,@Name,@LinkSeo,@Icon,@ParentId,@Ordering", parameters);
        }

        public HomeMenu Find(int id)
        {
            return _entities.HomeMenus.Find(id);
        }

        public IEnumerable<HomeMenu> GetAll()
        {
            return _entities.HomeMenus;
        }
    }
}
