using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Data.MediaContent;
using MyDeckAPI.Exceptions;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Services
{
    public class CardRepository : IGenericRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<Card> table;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public CardRepository(MDContext context, SnakeCaseConverter snakeCaseConverter)
        {
            _context = context;
            table = _context.Set<Card>();
            this.snakeCaseConverter = snakeCaseConverter;
        }
        public void Delete(IEnumerable<Card> cards)
        {
            table.RemoveRange(cards);
            _context.SaveChangesAsync();
        }

        public List<Card> FindAll()
        {
            return table.ToList();
        }

        public Card FindById(object Id)
        {
            return table.Find(Id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task<string> InsertAsync(IEnumerable<Card> obj)
        {
            try
            {
                var list = new List<Card>();
                var exceptionList = new List<Card>();
                foreach (Card card in obj)
                {
                    if (card.IsValid())
                    {
                        if (await _context.Files.FindAsync(card.Answer) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Answer });
                        }
                        if (await _context.Files.FindAsync(card.Question) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Question });
                        }
                        list.Add(card);
                        await _context.Cards.AddAsync(card);
                    }
                    else
                    {
                        exceptionList.Add(card);
                    }
                }
                await _context.SaveChangesAsync();
                if (exceptionList.Count > 0) { throw new NonValidatedModelException<Card>(exceptionList); }
                return snakeCaseConverter.ConvertToSnakeCase(list);
            }
            catch { throw; }
        }

        public async Task<string> UpdateAsync(IEnumerable<Card> obj)
        {
            try
            {
                var list = new List<Card>();
                var exceptionList = new List<Card>();
                foreach (Card card in obj)
                {
                    if (card.IsValid())
                    {
                        if (await _context.Files.FindAsync(card.Answer) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Answer });
                        }
                        if (await _context.Files.FindAsync(card.Question) == null)
                        {
                            await _context.Files.AddAsync(new File() { File_Id = card.Question });
                        }
                        list.Add(card);
                        _context.Cards.Update(card);
                    }
                    else
                    {
                        exceptionList.Add(card);
                    }
                }
                await _context.SaveChangesAsync();
                if (exceptionList.Count > 0) { throw new NonValidatedModelException<Card>(exceptionList); }
                return snakeCaseConverter.ConvertToSnakeCase(list);
            }
            catch { throw; }
        }

    }
}
