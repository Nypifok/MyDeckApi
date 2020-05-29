using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class User
    {
        [Key]
        public Guid User_Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public string GoogleId { get; set; }
        public string Password { get; set; }
        public string Avatar_Path { get; set; }
        public string Locale { get; set; }
        public string RefreshToken { get; set; }
        public string Sault { get; set; }
        public string Role_Name { get; set; }
        public Role Role { get; set; }
        public ICollection<Subscribe> Followers { get; set; }
        public ICollection<Subscribe> Publishers { get; set; }
        public ICollection<UserDeck> UserDecks { get; set; }
        public User()
        {
            UserDecks = new List<UserDeck>();
            Publishers = new List<Subscribe>();
            Followers = new List<Subscribe>();
        }


    }
}
