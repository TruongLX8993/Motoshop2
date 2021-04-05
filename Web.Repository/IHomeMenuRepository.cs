using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IHomeMenuRepository
    {
        IEnumerable<HomeMenu> GetAll();
        HomeMenu Find(int id);
        void Add(HomeMenu obj);
        void Edit(HomeMenu obj);
        void Delete(int id);
    }
}
