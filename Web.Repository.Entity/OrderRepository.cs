using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Web.Model;

namespace Web.Repository.Entity
{
    public class OrderRepository : IOrderRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
   
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.tbl_Order.Remove(obj);
            _entities.SaveChanges();
        }

        public int Add(tbl_Order model)
        {
            _entities.tbl_Order.Add(model);
            _entities.SaveChanges();
            return model.ID;
        }

        public void Edit(tbl_Order model)
        {
            var obj = Find(model.ID);
            object[] parameters =
            {
                new SqlParameter("@ID", model.ID),
                new SqlParameter("@Contents", model.Status)
            };
            _entities.Database.ExecuteSqlCommand("Sp_About_Update @ID,@MetaTitle,@Contents,@Tags", parameters);
        }

        public tbl_Order Find(int id)
        {
            return _entities.tbl_Order.Find(id);
        }

        public IEnumerable<tbl_Order> GetAll()
        { 
           return _entities.tbl_Order;
        } 
    }
}
