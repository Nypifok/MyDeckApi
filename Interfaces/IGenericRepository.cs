using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Interfaces
{
    public interface IGenericRepository<T> where T:class
    {
        List<T> FindAll();
        T FindById(object Id);
        void Insert(T obj);
        void Update(T obj);
        void Delete(object Id);
        void Save();
    }
}
