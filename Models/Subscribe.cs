using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Subscribe
    {
        public int Id { get; set; } 
        public int FollowerId { get; set; }
        public User Follower { get; set; }
        public int PublisherId { get; set; }
        public User Publisher { get; set; }





    }
}
