using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Web.Model;

namespace Web.Repository.Entity
{
    public class InstructionRepository : IInstructionRepository
    {
        readonly MotorEntities _entities = new MotorEntities();
       
        public bool Add(Instruction obj)
        {
            try
            {
                _entities.Instructions.Add(obj);
                _entities.SaveChanges();
                return true;
            }catch(Exception)
            {
                return false;
            }
           
        }

        public bool Delete(int id)
        {
            try
            {
                var obj = Find(id);
                _entities.Instructions.Remove(obj);
                _entities.SaveChanges();
                return true;
            }catch(Exception ex)
            {
                return false;
            }
           
        }

        public bool Edit(Instruction obj)
        {
            try
            {
               var model =  Find(obj.ID);
                model.Title = obj.Title;
                model.LinkSeo = obj.LinkSeo;
                model.Description = obj.Description;
                model.Contents = obj.Contents;
                model.CreatedDate = obj.CreatedDate;
                _entities.Entry(model);
                _entities.SaveChanges();
                return true;
            }catch(Exception)
            {
                return false;
           }
          
        }

        public Instruction Find(int id)
        {
            return _entities.Instructions.Find(id);
        }

        public IEnumerable<Instruction> GetAll()
        {
            var lstData = _entities.Instructions;
            return lstData;
        }
    }
}
