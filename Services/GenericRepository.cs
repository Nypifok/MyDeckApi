using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private MDContext _context;
        private DbSet<T> table;
        public GenericRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<T>();
        }
        public void Delete(object Id)
        {
            T exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<T> FindAll()
        {
            return table.ToList();
        }

        public T FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Update(obj);
            
        }
    }
}
