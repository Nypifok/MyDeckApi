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
        public Guid Card_Id { get; set; }
        public int Wins { get; set; }
        public int Trains { get; set; }
        public string Answer { get; set; }
        public string Question { get; set; }
        public int Lvl { get; set; }
        public DateTime Last_Train { get; set; }
        public Guid Parent_Deck_Id { get; set; }
        public Deck Parent_Deck { get; set; }
    }
}
