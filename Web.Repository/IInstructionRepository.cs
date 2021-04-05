using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository
{
    public interface IInstructionRepository
    {
        IEnumerable<Instruction> GetAll();
        Instruction Find(int id);
        bool Add(Instruction obj);
        bool Edit(Instruction obj);
        bool Delete(int id);
    }
}
