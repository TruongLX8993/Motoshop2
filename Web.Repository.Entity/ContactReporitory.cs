using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Web.Model;

namespace Web.Repository.Entity
{
    public class ContactReporitory : IInstructionReporitory
    {
        readonly MotorEntities _entities = new MotorEntities();
        public void Add(Contact obj)
        {
            _entities.Contacts.Add(obj);
            _entities.SaveChanges();
        }
        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.Contacts.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(Contact obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }

        public Contact Find(int id)
        {
            return _entities.Contacts.Find(id);
        }


        public IEnumerable<Contact> GetAll()
        {
            return _entities.Contacts;
        }
    }
}
