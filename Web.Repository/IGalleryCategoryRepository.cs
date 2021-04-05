using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IGalleryCategoryRepository
    {
        List<tbl_GalleryCategory> GetAll();
        tbl_GalleryCategory Find(int id);
        void Add(tbl_GalleryCategory obj);
        void Edit(tbl_GalleryCategory obj);
        void Delete(int id);
    }
}
