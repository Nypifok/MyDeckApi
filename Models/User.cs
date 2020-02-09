using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public int HashCode { get; set; }
        public ICollection<Subscribe> Followers { get; set; }
        public ICollection<Subscribe> Publishers { get; set; }
        public  ICollection<UserDeck> UserDecks { get; set; }
        public User()
        {
            UserDecks = new List<UserDeck>();
            Publishers = new List<Subscribe>();
            Followers = new List<Subscribe>();
        }
        
      
    }
}
