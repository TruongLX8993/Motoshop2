using System;
using System.Collections.Generic;
using Web.Model;

namespace Web.Repository
{
    public interface IOrderDetailRepository
    {
        IEnumerable<OrderDetail> GetAll();
        void Add(OrderDetail model);
        OrderDetail Find(int id);
        void Delete(int id);
    }
}
