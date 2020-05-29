﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyDeckAPI.Models;

namespace MyDeckAPI.Migrations
{
    [DbContext(typeof(MDContext))]
    [Migration("20200409090352_FixedSomeBugs")]
    partial class FixedSomeBugs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MyDeckAPI.Models.Card", b =>
                {
                    b.Property<Guid>("Card_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Answer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Last_Train")
                        .HasColumnType("datetime2");

                    b.Property<int>("Lvl")
                        .HasColumnType("int");

                    b.Property<Guid>("Parent_Deck_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Question")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Trains")
                        .HasColumnType("int");

                    b.Property<int>("Wins")
                        .HasColumnType("int");

                    b.HasKey("Card_Id");

                    b.HasIndex("Parent_Deck_Id");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("MyDeckAPI.Models.Category", b =>
                {
                    b.Property<string>("Category_Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Category_Name");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            Category_Name = "No category"
                        },
                        new
                        {
                            Category_Name = "Math"
                        },
                        new
                        {
                            Category_Name = "Foreign Languages"
                        },
                        new
                        {
                            Category_Name = "Chemistry"
                        },
                        new
                        {
                            Category_Name = "Art"
                        },
                        new
                        {
                            Category_Name = "IT"
                        },
                        new
                        {
                            Category_Name = "Others"
                        });
                });

            modelBuilder.Entity("MyDeckAPI.Models.Deck", b =>
                {
                    b.Property<Guid>("Deck_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Category_Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Icon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPrivate")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Deck_Id");

                    b.HasIndex("Category_Name");

                    b.ToTable("Decks");
                });

            modelBuilder.Entity("MyDeckAPI.Models.Role", b =>
                {
                    b.Property<string>("Role_Name")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Role_Name");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Role_Name = "Owner"
                        },
                        new
                        {
                            Role_Name = "Administrator"
                        },
                        new
                        {
                            Role_Name = "Support"
                        },
                        new
                        {
                            Role_Name = "Content Maker"
                        },
                        new
                        {
                            Role_Name = "User"
                        });
                });

            modelBuilder.Entity("MyDeckAPI.Models.Subscribe", b =>
                {
                    b.Property<Guid>("Follower_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Publisher_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Subscribe_Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Follower_Id", "Publisher_Id");

                    b.HasIndex("Publisher_Id");

                    b.ToTable("Subscribes");
                });

            modelBuilder.Entity("MyDeckAPI.Models.User", b =>
                {
                    b.Property<Guid>("User_Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Avatar_Path")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("GoogleId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Locale")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role_Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("User_Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Role_Name");

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("MyDeckAPI.Models.UserDeck", b =>
                {
                    b.Property<Guid>("User_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Deck_Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserDeck_Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("User_Id", "Deck_Id");

                    b.HasIndex("Deck_Id");

                    b.ToTable("UserDecks");
                });

            modelBuilder.Entity("MyDeckAPI.Models.Card", b =>
                {
                    b.HasOne("MyDeckAPI.Models.Deck", "Parent_Deck")
                        .WithMany("Cards")
                        .HasForeignKey("Parent_Deck_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyDeckAPI.Models.Deck", b =>
                {
                    b.HasOne("MyDeckAPI.Models.Category", "Category")
                        .WithMany("Decks")
                        .HasForeignKey("Category_Name");
                });

            modelBuilder.Entity("MyDeckAPI.Models.Subscribe", b =>
                {
                    b.HasOne("MyDeckAPI.Models.User", "Follower")
                        .WithMany("Publishers")
                        .HasForeignKey("Follower_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MyDeckAPI.Models.User", "Publisher")
                        .WithMany("Followers")
                        .HasForeignKey("Publisher_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MyDeckAPI.Models.User", b =>
                {
                    b.HasOne("MyDeckAPI.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("Role_Name");
                });

            modelBuilder.Entity("MyDeckAPI.Models.UserDeck", b =>
                {
                    b.HasOne("MyDeckAPI.Models.Deck", "Deck")
                        .WithMany("UserDecks")
                        .HasForeignKey("Deck_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyDeckAPI.Models.User", "User")
                        .WithMany("UserDecks")
                        .HasForeignKey("User_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}