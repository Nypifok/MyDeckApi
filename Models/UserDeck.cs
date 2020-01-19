using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class UserDeck
    {
        public int Id { get; set; }

        
      //  public int UserId { get; set; }
        public User User { get; set; }

        
       // public int DeckId { get; set; } 
        public Deck Deck { get; set; }
    }
}
