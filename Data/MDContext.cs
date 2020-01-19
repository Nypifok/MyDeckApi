using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
namespace MyDeckAPI.Models
{
    public class MDContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Deck> Decks { get; set; }
        public DbSet<Subscribe> Subscribes { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<UserDeck> UserDecks { get; set; }


        public MDContext(DbContextOptions<MDContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<UserDeck>()
                        .HasOne(ud => ud.User)
                        .WithMany(u => u.UserDecks)
                        .HasForeignKey(ud => ud.UserId);
            modelBuilder.Entity<UserDeck>()
                        .HasOne(ud => ud.Deck)
                        .WithMany(d => d.UserDecks)
                        .HasForeignKey(ud => ud.DeckId);
            modelBuilder.Entity<Subscribe>()
                        .HasOne(fp => fp.Follower)
                        .WithMany(f => f.Publishers)
                        .HasForeignKey(fp => fp.FollowerId)
                        .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscribe>()
                        .HasOne(fp => fp.Publisher)
                        .WithMany(p => p.Followers)
                        .HasForeignKey(fp => fp.PublisherId)
                        .OnDelete(DeleteBehavior.Cascade);
            
               
        }
    }
}
