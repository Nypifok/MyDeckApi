using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MyDeckAPI.Data.MediaContent;

namespace MyDeckAPI.Services
{
    public class DeckRepository : IGenericRepository
    {
        private readonly MDContext _context;
        private readonly DbSet<Deck> table;
        private readonly SnakeCaseConverter snakeCaseConverter;

        public DeckRepository(MDContext context, SnakeCaseConverter snakeCaseConverter)
        {
            _context = context;
            table = _context.Set<Deck>();
            this.snakeCaseConverter = snakeCaseConverter;
        }

        public void Delete(IEnumerable<Deck> decks)
        {
            table.RemoveRange(decks);
            _context.SaveChangesAsync();
        }

        public List<Deck> FindAll()
        {
            return table.ToList();
        }

        public Deck FindById(object Id)
        {
            return table.Find(Id);
        }

        /*public async Task<string> InsertAsync(IEnumerable<Deck> obj)
        {
            
        }*/

        public void Save()
        {
            _context.SaveChanges();
        }

       /* public async Task<string> Update(FilledDeck obj)
        {
            var filler = new DeckFiller();
            Models.Deck dck = (Models.Deck)await filler.ConvertToModel(obj,contentSaver);
            _context.Decks.Update(dck);
            return snakeCaseConverter.ConvertToSnakeCase(dck);
        }*/
        public string AllCurrentUserDecks(string login)
        {
            var content = _context.Decks.Where(d => d.Author == login && d.IsPrivate == false).ToList();
            return snakeCaseConverter.ConvertToSnakeCase(content);
        }
        public string AllCurrentUserDecksWithCards(string login)
        {
            var content = _context.Decks.Where(d => d.Author == login && d.IsPrivate == false).Include(c => c.Cards).ToList();
            return snakeCaseConverter.ConvertToSnakeCase(content);
        }
        public string ChosenCategoryFeed(string categoryname, int pagenumber = 0)
        {
            var decks = _context.Decks.Where(d => d.Category_Name == categoryname && d.IsPrivate == false)
                                    .Skip(pagenumber * 15)
                                    .Take(15)
                                    .Select(d => new
                                    {
                                        d.Deck_Id,
                                        d.Title,
                                        d.Description,
                                        d.IsPrivate,
                                        d.Icon,
                                        d.Category_Name,
                                        d.Author,
                                        Cards_Count = d.Cards.Count,
                                        Subscribers_Count = _context.UserDecks.Where(ud => ud.Deck_Id == d.Deck_Id).Count()
                                    })
                                    .ToList();

            return snakeCaseConverter.ConvertToSnakeCase(decks);
        }
        public string AllUserDecks(Guid id)
        {
            var decks = from userdeck in _context.Set<UserDeck>().Where(u=>u.User_Id==id)
                        join deck in _context.Set<Models.Deck>().Include(d=>d.Cards)
                            on userdeck.Deck_Id equals deck.Deck_Id
                        select  deck ;



            return snakeCaseConverter.ConvertToSnakeCase(decks);
        }
        /*public string WatchDeck(Guid id)
        {
            var deck = _context.Decks.Where(d => d.Deck_Id == id).Include(d => d.Cards).FirstOrDefault();
            //TODO REFACTORING
            var repo = new UserRepository(_context,snakeCaseConverter);
            var usr = JsonConvert.DeserializeObject(repo.UserProfile(Guid.Parse(deck.Author)));
            return snakeCaseConverter.ConvertToSnakeCase(new { Deck = deck, Author = usr });
        }*/
    }
}
