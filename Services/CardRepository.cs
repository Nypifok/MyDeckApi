using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class CardRepository<Card> : IGenericRepository<Card> where Card : class
    {
        private MDContext _context;
        private DbSet<Card> table;
        public CardRepository(MDContext context)
        {
            _context = context;
            table = _context.Set<Card>();
        }
        public void Delete(object Id)
        {
            Card exists = table.Find(Id);
            table.Remove(exists);
        }

        public List<Card> FindAll()
        {
            return table.ToList();
        }

        public Card FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Insert(Card obj)
        {
             table.Add(obj);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Card obj)
        {
            table.Update(obj);
        }
    }
}
