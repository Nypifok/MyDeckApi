using Microsoft.EntityFrameworkCore;
using MyDeckAPI.Interfaces;
using MyDeckAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

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

        public void Insert(Models.Deck obj)
        {
           
            _context.Decks.Add(obj);
            var usrdck = new UserDeck { Deck_Id = obj.Deck_Id, User_Id = Guid.Parse(obj.Author) };
            _context.UserDecks.Add(usrdck);
      
        }
        public void Insert(Deck obj)
        {
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public void Update(Deck obj)
        {
            table.Update(obj);
        }
        public string AllCurrentUserDecks(string login)
        {
            var content = _context.Decks.Where(d => d.Author == login && d.IsPrivate == false).ToList();
            return JsonConvert.SerializeObject(content);
        }
        public string AllCurrentUserDecksWithCards(string login)
        {
            var content = _context.Decks.Where(d => d.Author == login && d.IsPrivate == false).Include(c => c.Cards).ToList();
            return JsonConvert.SerializeObject(content, Formatting.Indented,
                new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects
                });
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

            return JsonConvert.SerializeObject(decks);
        }
        public string AllUserDecks(Guid id)
        {
            var decks = from userdeck in _context.Set<UserDeck>().Where(u=>u.User_Id==id)
                        join deck in _context.Set<Models.Deck>().Include(d=>d.Cards)
                            on userdeck.Deck_Id equals deck.Deck_Id
                        select  deck ;



            return JsonConvert.SerializeObject(decks, Formatting.Indented,
                 new JsonSerializerSettings
                 {
                     PreserveReferencesHandling = PreserveReferencesHandling.Objects
                 });
        }
        public string WatchDeck(Guid id)
        {
            var deck = _context.Decks.Where(d => d.Deck_Id == id).Include(d => d.Cards).FirstOrDefault();
            var repo = new UserRepository<User>(_context);
            var usr = JsonConvert.DeserializeObject(repo.UserProfile(Guid.Parse(deck.Author)));



            return JsonConvert.SerializeObject(new { Deck=deck,Author=usr }, Formatting.Indented,
                 new JsonSerializerSettings
                 {
                     PreserveReferencesHandling = PreserveReferencesHandling.Objects
                 });
        }
    }
}
