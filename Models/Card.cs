using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FrontContent { get; set; }
        public string BackContent { get; set; }
        public int Frequency { get; set; }
        public DateTime LastTrainDate { get; set; }


        public int DeckId { get; set; }
        public Deck Deck { get; set; }
    }
}
