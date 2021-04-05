using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IGalleryRepository
    {
        IEnumerable<tbl_Gallery> GetAll();
        tbl_Gallery Find(int id);
        void Add(tbl_Gallery obj);
        void Edit(tbl_Gallery obj);
        void Delete(int id);
    }
}
