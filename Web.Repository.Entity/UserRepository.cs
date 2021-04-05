using System;
using System.Collections.Generic;
using System.Data.Entity;
using Web.Model;

namespace Web.Repository.Entity
{
    public class UserRepository : IUserRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
        public void Add(User obj)
        {
            _entities.Users.Add(obj);
            _entities.SaveChanges();
        }

        public void Delete(int id)
        {
            var obj = Find(id);
            _entities.Users.Remove(obj);
            _entities.SaveChanges();
        }
        public void Edit(User obj)
        {
            _entities.Entry(obj).State = EntityState.Modified;
            _entities.SaveChanges();
        }
        public void Edit(List<User> lstobj)
        {
            foreach (var obj in lstobj)
            {
                _entities.Entry(obj).State = EntityState.Modified;
            }
            _entities.SaveChanges();
        }
        public User Find(int id)
        {
            return _entities.Users.Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return _entities.Users;
        }
    }
}
