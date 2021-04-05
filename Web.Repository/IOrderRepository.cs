using System;
using System.Collections.Generic;
using Web.Model;

namespace Web.Repository
{
    public interface IOrderRepository
    {
        IEnumerable<tbl_Order> GetAll();
        void Delete(int id);
        tbl_Order Find(int id);
        int Add(tbl_Order model);
        void Edit(tbl_Order model);
    }
}
