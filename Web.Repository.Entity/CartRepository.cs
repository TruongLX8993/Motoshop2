using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Core;
using Web.Model;
using Web.Model.CustomModel;

namespace Web.Repository.Entity
{
    public class CartRepository : ICartRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
        private const string KeyCache = "cachecategories"; 
        private const string KeyCacheView = "cachecategories_view";
        public IEnumerable<tbl_Order> GetAll()
        {
            return _entities.tbl_Order;
        }
        public void AddOrderDetail(OrderDetail model)
        {
            HelperCache.RemoveCache(KeyCache);
            object[] parameters =
            {
                new SqlParameter("@ProductID",model.ProductID),
                new SqlParameter("@OrderID", model.OrderID),
                new SqlParameter("@Quantity", model.Quantity) 
            };
            _entities.Database.ExecuteSqlCommand("Sp_OrderDetail_Insert @ProductID,@OrderID,@Quantity", parameters);
        }
       
    }
}
