using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class UserRepository<User> : IGenericRepository<User> where User:class
    {
        private MDContext _context;
        private DbSet<User> table;

        public UserRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<User>();
        }

        public void Delete(object Id)
        {
            var subscriptions = _context.Subscribes.Where(p=>p.PublisherId==(Int32)Id);
            _context.Subscribes.RemoveRange(subscriptions);
            subscriptions = _context.Subscribes.Where(f => f.FollowerId == (Int32)Id);
            _context.Subscribes.RemoveRange(subscriptions);
            User exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<User> FindAll()
        {
            return table.ToList();
        }

        public User FindById(object Id)
        {
           
           
            return table.Find(Id);
            
            
        }

        public void Insert(User obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(User obj)
        {
            table.Update(obj);
        }
    }
}
