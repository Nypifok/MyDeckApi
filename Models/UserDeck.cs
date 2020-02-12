using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class UserDeck
    {
        [Key]
        public Guid Id { get; set; }

        
        public Guid UserId { get; set; }
        public User User { get; set; }

        
        public Guid DeckId { get; set; } 
        public Deck Deck { get; set; }
    }
}
