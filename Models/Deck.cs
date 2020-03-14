using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Deck
    {
        [Key]
        public Guid Deck_Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public string Icon { get; set; }
        [NotMapped]
        private string category_Name { get; set; }
        public string Category_Name { get { return category_Name; } set { if (value == null) { category_Name = "No category"; } else { category_Name = value; } } }
        public Category Category { get; set; }
        public string Author { get; set; }
        public ICollection<UserDeck> UserDecks { get; set; }
        public ICollection<Card> Cards { get; set; }
        public Deck()
        {
            UserDecks = new List<UserDeck>();
            Cards = new List<Card>();
        }
       
    }
}
