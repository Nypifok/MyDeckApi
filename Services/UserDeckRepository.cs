using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class UserDeckRepository<UserDeck> : IGenericRepository<UserDeck> where UserDeck : class
    {
        private MDContext _context;
        private DbSet<UserDeck> table;

        public UserDeckRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<UserDeck>();
        }

        public void Delete(object Id)
        {
            UserDeck exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<UserDeck> FindAll()
        {
            return table.ToList();
        }

        public UserDeck FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Insert(UserDeck obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(UserDeck obj)
        {
            table.Update(obj);

        }
        
    }
}
