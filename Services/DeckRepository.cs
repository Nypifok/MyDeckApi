using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class DeckRepository<Deck> : IGenericRepository<Deck> where Deck : class
    {
        private MDContext _context;
        private DbSet<Deck> table;

        public DeckRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<Deck>();
        }

        public void Delete(object Id)
        {
           
            Deck exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<Deck> FindAll()
        {
            return table.ToList();
        }

        public Deck FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Insert(Deck obj)
        {
            table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Deck obj)
        {
            table.Update(obj);
        }
    }
}
