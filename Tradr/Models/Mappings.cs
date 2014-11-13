using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace Tradr.Models
{
    public class OfferMapping : EntityTypeConfiguration<Offer>
    {
        public OfferMapping()
        {
            this.HasRequired(t => t.Sender)
                .WithMany(s => s.SentOffers)
                .HasForeignKey(t => t.SenderId)
                .WillCascadeOnDelete(false); ;
            this.HasRequired(t => t.Reciever)
                .WithMany(s => s.RecievedOffers)
                .HasForeignKey(t => t.RecieverId)
                .WillCascadeOnDelete(false); ;
        }
    }

    public class ItemMapping : EntityTypeConfiguration<Item>
    {
        public ItemMapping()
        {
            this.HasRequired(t => t.User)
                .WithMany(s => s.Items)
                .HasForeignKey(t => t.UserId)
                .WillCascadeOnDelete(false); ;

            this.HasMany(t => t.Tags)
                .WithMany(s => s.ItemTagsTags)
                .Map(m =>
                {
                    m.ToTable("ItemTagsTags");
                    m.MapLeftKey("ItemId");
                    m.MapRightKey("TagId");
                });
            this.HasMany(t => t.Wants)
                .WithMany(s=> s.ItemWantsTags)
                .Map(m =>
                {
                    m.ToTable("ItemWantsTags");
                    m.MapLeftKey("ItemId");
                    m.MapRightKey("TagId");
                });
            this.HasMany(t => t.ProposedItem)
                .WithMany(s => s.ProposedItems)
                .Map(m =>
                {
                    m.ToTable("ProposedItemOffer");
                    m.MapLeftKey("ItemId");
                    m.MapRightKey("OfferId");
                });
            this.HasMany(t => t.DesiredItem)
                .WithMany(s => s.DesiredItems)
                .Map(m =>
                {
                    m.ToTable("DesiredItemOffer");
                    m.MapLeftKey("ItemId");
                    m.MapRightKey("OfferId");
                });
        }
    }
}