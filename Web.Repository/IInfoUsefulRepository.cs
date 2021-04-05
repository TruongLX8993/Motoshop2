﻿
using System;
using System.Collections.Generic;
using Web.Model;

namespace Web.Repository
{
    public interface IInfoUsefulRepository
    {
        IEnumerable<InfoUseful> GetAll();
        void Add(InfoUseful model);
        void Delete(int id);
        InfoUseful Find(int id);
        void Edit(InfoUseful model);
    }
}


