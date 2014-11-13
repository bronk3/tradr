using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Tradr.Models;

namespace Tradr.DAL
{
    public class TradrContext : DbContext   
    {
        public TradrContext() : base("TradrContext")
    {
    }
        public DbSet<Item> Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Offer> Offers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ItemMapping());
            modelBuilder.Configurations.Add(new OfferMapping());
            base.OnModelCreating(modelBuilder);
        }

    }
}