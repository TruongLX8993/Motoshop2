using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IInstructionReporitory
    {
        IEnumerable<Contact> GetAll();
        Contact Find(int id);
        void Add(Contact obj);
        void Edit(Contact obj);
        void Delete(int id);
    }
}
