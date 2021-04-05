using System.Collections.Generic;
using Web.Model;

namespace Web.Repository
{
    public interface IHeaderRepository
    {
        IEnumerable<Header> GetAll();
        void Add(string content);
        Header Find(int id);
        void Edit(Header model);
    }
}
