using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class SubscribeRepository<Subscribe> : IGenericRepository<Subscribe> where Subscribe : class
    {
        private MDContext _context;
        private DbSet<Subscribe> table;
        public SubscribeRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<Subscribe>();
        }

        public void Delete(object Id)
        {
            Subscribe exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<Subscribe> FindAll()
        {
            return table.ToList();
        }

        public Subscribe FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Insert(Subscribe obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Subscribe obj)
        {
            table.Update(obj);

        }
    }
}
