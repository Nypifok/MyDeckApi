using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Card
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string FrontContent { get; set; }
        public string BackContent { get; set; }
        public int Frequency { get; set; }
        public DateTime LastTrainDate { get; set; }
        public Guid DeckId { get; set; }
        public Deck Deck { get; set; }
    }
}
