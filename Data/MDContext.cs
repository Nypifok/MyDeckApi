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
           // Database.EnsureDeleted();
            Database.EnsureCreated();

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasKey(c => c.Category_Name);
            modelBuilder.Entity<UserDeck>()
                .HasKey(ud => new { ud.User_Id,ud.Deck_Id });

            modelBuilder.Entity<UserDeck>()
                        .HasOne(ud => ud.User)
                        .WithMany(u => u.UserDecks)
                        .HasForeignKey(ud => ud.User_Id)
                        .OnDelete(DeleteBehavior.Cascade);
                        
            modelBuilder.Entity<UserDeck>()
                        .HasOne(ud => ud.Deck)
                        .WithMany(d => d.UserDecks)
                        .HasForeignKey(ud => ud.Deck_Id)
                        .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Subscribe>().HasKey(s => new { s.Follower_Id, s.Publisher_Id });
            modelBuilder.Entity<Subscribe>()
                        .HasOne(s => s.Follower)
                        .WithMany(f => f.Publishers)
                        .HasForeignKey(s => s.Follower_Id)
                        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Subscribe>()
                        .HasOne(s => s.Publisher)
                        .WithMany(p => p.Followers)
                        .HasForeignKey(s => s.Publisher_Id)
                        .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Deck>()
                        .HasOne(c => c.Category)
                        .WithMany(d => d.Decks)
                        .HasForeignKey(k => k.Category_Name);
            modelBuilder.Entity<Card>()
                       .HasOne(d => d.Parent_Deck)
                       .WithMany(c => c.Cards)
                       .HasForeignKey(d => d.Parent_Deck_Id);

            modelBuilder.Entity<Category>().HasData(new Category { Category_Name = "No category" },
                                                    new Category { Category_Name = "Math"},
                                                    new Category { Category_Name = "Foreign Languages" },
                                                    new Category { Category_Name = "Chemistry" },
                                                    new Category { Category_Name = "Art" },
                                                    new Category { Category_Name = "IT" },
                                                    new Category { Category_Name = "Others" }        );

            modelBuilder.Entity<User>().HasIndex(u => u.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();


        }
    }
}
