using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Category
    {
        [Key]
        [Required]
        public string Category_Name { get; set; }
        public ICollection<Deck> Decks { get; set; }
        public Category()
        {
            Decks = new List<Deck>();
        }
    }
}
