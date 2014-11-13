using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using Microsoft.Ajax.Utilities;

namespace Tradr.Models
{

    public class ViewLogin
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
    }

    public class ViewItem
    {
        public string Images { get; set; }

        public string Title { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }

        public string Tags { get; set; }
        public string Wants { get; set; }

    }

    public class Item
    {
        public int ItemId { get; set; }
        public string Title { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public DateTime DateTimeAdded { get; set; }
        public int Views { get; set; }
        public ItemStatus Status { get; set; }

        public virtual User User { get; set; }
        public int UserId { get; set; }

        public virtual ICollection<Offer> ProposedItem { get; set; }
        public virtual ICollection<Offer> DesiredItem { get; set; }
        
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Tag> Wants { get; set; }

        public virtual ICollection<Image> Images { get; set; } 

        public Item()
        {
            //User = new User();
            ProposedItem = new List<Offer>();
            DesiredItem = new List<Offer>();
            Tags = new List<Tag>();
            Wants = new List<Tag>();
            DateTimeAdded = DateTime.UtcNow;
            Views = 0;
            Status = ItemStatus.Available;
        }
    }

    public class Image
    {
        public int ImageId { get; set; }
        public string ItemImage { get; set; }

        public Item Item { get; set; }
    }

    public class Tag
    {
        public int TagId { get; set; }
        public string TagName { get; set; }

        public virtual ICollection<Item> ItemTagsTags { get; set; }
        public virtual ICollection<Item> ItemWantsTags { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public Tag()
        {
            ItemTagsTags = new List<Item>();
            ItemWantsTags = new List<Item>();
            Users = new List<User>();
        }
    }

    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public DateTime DateTimeRegistered { get; set; }

        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string AreaCode { get; set; }

        public virtual ICollection<Tag> Wants { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Offer> SentOffers { get; set; }
        public virtual ICollection<Offer> RecievedOffers { get; set; }

        public User()
        {
            Wants = new List<Tag>();
            Items = new List<Item>();
            SentOffers = new List<Offer>();
            RecievedOffers = new List<Offer>();
            DateTimeRegistered = DateTime.UtcNow;
        }
    }

    public class Offer
    {
        public int OfferId { get; set; }
        public DateTime DateTimeInitial { get; set; }
        public OfferStatus Status { get; set; }
        public OfferSee See { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Reciever { get; set; }

        public int SenderId { get; set; }
        public int RecieverId { get; set; }

        public virtual ICollection<Item> ProposedItems { get; set; }
        public virtual ICollection<Item> DesiredItems { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public Offer()
        {
            ProposedItems = new List<Item>();
            DesiredItems = new List<Item>();
            Messages = new List<Message>();
            DateTimeInitial = DateTime.UtcNow;
        }
    }



    public class Message
    {
        public int MessageId { get; set; }
        public DateTime DateTimeMessage { get; set; }
        public string MessageText { get; set; }

        public virtual Offer Offer { get; set; }

        public virtual User Sender { get; set; }
        public virtual User Reciever { get; set; }

        public Message()
        {
            //Offer = new Offer();
            //Sender = new User();
            //Reciever = new User();
            DateTimeMessage = DateTime.UtcNow;
        }
    }

    public enum OfferStatus
    {
        Negotiation,
        Rejected,
        Accepted,
        Initial
    }

    public enum ItemStatus
    {
        Available,
        Negotiation,
        Unavailable
    }

    public enum OfferSee
    {
        NotSeen,
        Seen
    }
}