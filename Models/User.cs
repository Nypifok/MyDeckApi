﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Icon { get; set; }
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
