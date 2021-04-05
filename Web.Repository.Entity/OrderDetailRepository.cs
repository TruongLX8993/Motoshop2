using System;
using System.Collections.Generic;
using Web.Model;

namespace Web.Repository.Entity
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        readonly MotorEntities _entities = new MotorEntities();

        public IEnumerable<OrderDetail> GetAll()
        {
            return _entities.OrderDetails;
        }

        public void Add(OrderDetail model)
        {
            _entities.OrderDetails.Add(model);
            _entities.SaveChanges();
        }

        public OrderDetail Find(int id)
        {
            return _entities.OrderDetails.Find(id);
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.OrderDetails.Remove(obj);
            _entities.SaveChanges();
        }
    }
}
