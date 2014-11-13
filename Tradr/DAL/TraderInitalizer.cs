using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using Tradr.Models;

namespace Tradr.DAL
{
    public class TraderInitalizer : DropCreateDatabaseIfModelChanges<TradrContext>
    {
        protected override void Seed(TradrContext context)
        {

           
            //Users

            var kari = new User()
            {
                FirstName = "Kari",
                LastName = "Bronson",
                AreaCode = "T5k",
                City = "Edmonton",
                EmailAddress = "karibronson10@gmail.com",
                Wants =
                    new List<Tag>
                    {
                        new Tag() {TagName = "Cats"},
                        new Tag() {TagName = "Dogs"},
                        new Tag() {TagName = "Cars"},
                        new Tag() {TagName = "Socks"}
                    },
                Password = "password",
                Phone = "7809748246"
            };


            var jack = new User()
            {
                FirstName = "Jack",
                LastName = "Black",
                AreaCode = "T6C",
                City = "Edmonton",
                EmailAddress = "JackBlack@gmail.com",
                Wants =
                    new List<Tag>
                    {
                        new Tag() {TagName = "knives"},
                        new Tag() {TagName = "badges"},
                        new Tag() {TagName = "Jackets"},
                        new Tag() {TagName = "Guns"}
                    },
                Password = "password123",
                Phone = "7809953456"
            };
            context.Users.Add(kari);
            context.Users.Add(jack);
            foreach (var tags in jack.Wants)
            {
                context.Tags.Add(tags);
            }
            foreach (var tags in kari.Wants)
            {
                context.Tags.Add(tags);
            }
            context.SaveChanges();

            //Items
            var fordChevy = new Item()
            {
                Title = "Brand New Ford Chevy",
                Description = "Clean interior",
                Images = new List<Image> { new Image() { ItemImage = "sample.png"}},
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Truck" }, new Tag() { TagName = "2014" }, new Tag() { TagName = "Blue" } },
                Wants = new List<Tag> { new Tag() { TagName = "guns" } },
                Value = "40000",
                User = kari
            };
            var shotgun = new Item()
            {
                Title = "Revolver",
                Description = "Fancy Gun",
                Images = new List<Image> { new Image() { ItemImage = "caesar-salad-twist.jpg"}},
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Gun" }, new Tag() { TagName = "Revolver" }, new Tag() { TagName = "Silver" } },
                Wants = new List<Tag> { new Tag() { TagName = "cars" } },
                Value = "20000",
                User = jack
            };
            context.Items.Add(fordChevy);
            context.Items.Add(shotgun);
            context.SaveChanges();

            //Messages
            var jackkariFirstMessage = new Message()
            {
                Reciever = kari,
                Sender = jack,
                MessageText = "What is the milage on your chevey?"
            };


            //Offers set Items to "negotiation" as well
            var jackari = new Offer()
            {
                Sender = jack,
                Reciever = kari,
                DesiredItems = new List<Item>{ fordChevy },
                ProposedItems = new List<Item> { shotgun },
                Messages = new List<Message> { new Message() { MessageText = "Hi I want to trade you this shotgun for the car are you interested?" } },
                Status = OfferStatus.Negotiation
            };

            foreach (var message in jackari.Messages)
            {
                context.Messages.Add(message);
            }

            context.Messages.Add(jackkariFirstMessage);
            context.Offers.Add(jackari);

            context.SaveChanges();

        }
    }
}