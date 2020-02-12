using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyDeckAPI.Models
{
    public class Subscribe
    {
        [Key]
        public Guid Id { get; set; } 
        public Guid FollowerId { get; set; }
        public User Follower { get; set; }
        public Guid PublisherId { get; set; }
        public User Publisher { get; set; }





    }
}
