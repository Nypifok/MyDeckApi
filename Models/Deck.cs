using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Deck
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool Privacy { get; set; }
        public string Icon { get; set; }
        public int AuthorId { get; set; }
        public User Author { get; set; }
        public  ICollection<UserDeck> UserDecks { get; set; }
        public ICollection<Card> Cards { get; set; }
      
       
    }
}
