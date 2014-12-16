using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using Tradr.Controllers;
using Tradr.Models;

namespace Tradr.DAL
{
    public class TraderInitalizer : DropCreateDatabaseIfModelChanges<TradrContext>
    {
        protected override void Seed(TradrContext context)
        {
            var loginController = new LoginController();
           
            //Users
            var snowman = new User()
            {
                FirstName = "snowman",
                LastName = "snowman",
                AreaCode = "T5k",
                City = "snowman",
                EmailAddress = "snowman",
                Wants =
                    new List<Tag>
                    {
                        new Tag() {TagName = "snowman"},
                        new Tag() {TagName = "Dogs"},
                        new Tag() {TagName = "Cars"},
                        new Tag() {TagName = "Socks"}
                    },
                Password = loginController.CreateHash("snowman"),
                Phone = "7809748246"
            };

            var admin = new User()
            {
                FirstName = "admin",
                LastName = "Admin",
                AreaCode = "T5k",
                City = "Admin",
                EmailAddress = "admin",
                Wants =
                    new List<Tag>
                    {
                        new Tag() {TagName = "Cats"},
                        new Tag() {TagName = "Dogs"},
                        new Tag() {TagName = "Cars"},
                        new Tag() {TagName = "Socks"}
                    },
                Password = loginController.CreateHash("admin"),
                Phone = "7809748246"
            };

            var kari = new User()
            {
                FirstName = "ashley",
                LastName = "Brown",
                AreaCode = "T5k",
                City = "Edmonton",
                EmailAddress = "ashley",
                Wants =
                    new List<Tag>
                    {
                        new Tag() {TagName = "Cats"},
                        new Tag() {TagName = "Dogs"},
                        new Tag() {TagName = "Cars"},
                        new Tag() {TagName = "Socks"}
                    },
                Password = loginController.CreateHash("ashley"),
                Phone = "7809748246"
            };


            var jack = new User()
            {
                FirstName = "Jack",
                LastName = "Black",
                AreaCode = "T6C",
                City = "Edmonton",
                EmailAddress = "jack",
                Wants =
                    new List<Tag>
                    {
                        new Tag() {TagName = "knives"},
                        new Tag() {TagName = "badges"},
                        new Tag() {TagName = "Jackets"},
                        new Tag() {TagName = "Guns"}
                    },
                Password = loginController.CreateHash("jack"),
                Phone = "7809953456"
            };
            context.Users.Add(kari);
            context.Users.Add(jack);
            context.Users.Add(admin);
            context.Users.Add(snowman);

            foreach (var tags in jack.Wants)
            {
                context.Tags.Add(tags);
            }
            foreach (var tags in kari.Wants)
            {
                context.Tags.Add(tags);
            }
            foreach (var tags in admin.Wants)
            {
                context.Tags.Add(tags);
            }
            foreach (var tags in snowman.Wants)
            {
                context.Tags.Add(tags);
            }
            context.SaveChanges();

            //Items
            var fordChevy = new Item()
            {
                Title = "Brand New Ford Chevy",
                Description = "Clean interior",
                Images = new List<Image> { new Image() { ItemImage = "fordchevy.jpg"}},
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Truck" }, new Tag() { TagName = "2014" }, new Tag() { TagName = "Blue" } },
                Wants = new List<Tag> { new Tag() { TagName = "guns" } },
                Value = "40000",
                User = kari
            };
            var shotgun = new Item()
            {
                Title = "Shot Gun",
                Description = "Fancy Gun",
                Images = new List<Image> { new Image() { ItemImage = "shotgun.jpg"}},
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Gun" }, new Tag() { TagName = "Shot Gun" }, new Tag() { TagName = "Silver" } },
                Wants = new List<Tag> { new Tag() { TagName = "cars" } },
                Value = "20000",
                User = jack
            };
            var redtruck1 = new Item()
            {
                Title= "Red Ford Chevy Pickup Truck",
                Description = "Lorem ipsum dolor Red Truck Automatic sit amet, consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "redtruck1.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> {new Tag() {TagName = "Truck"}, new Tag() {TagName = "Red"}, new Tag(){TagName="Automatic"}},
                Wants = new List<Tag>{ new Tag() {TagName = "Lamp"}, new Tag() {TagName = "BBQ"}, new Tag(){TagName = "Cats"}},
                Value = "18000",
                User = admin
            };
            var redtruck2 = new Item()
            {
                Title = "Red Fire Truck",
                Description = "Lorem ipsum dolor Red Diesel sit amet, Truck consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "redtruck2.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Truck" }, new Tag() { TagName = "Red" }, new Tag() { TagName = "Diesel" } },
                Wants = new List<Tag> { new Tag() { TagName = "Lamp" }, new Tag() { TagName = "makeup" }, new Tag() { TagName = "Cats" } },
                Value = "19000",
                User = snowman
            };
            var redtruck3 = new Item()
            {
                Title = "Red Toyota Pickup Truck",
                Description = "Lorem ipsum dolor Red  sit amet, Truck consectetur Standard Transmition adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "redtruck3.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Truck" }, new Tag() { TagName = "Red" }, new Tag() { TagName = "Standard" } },
                Wants = new List<Tag> { new Tag() { TagName = "Lamp" }, new Tag() { TagName = "BBQ" }, new Tag() { TagName = "Necklaces" } },
                Value = "19000",
                User = jack
            };
            var necklace = new Item()
            {
                Title = "15k Gold Necklace",
                Description = "Lorem ipsum dolor  Gold sit amet,  consectetur  adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "necklace1.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Necklace" }, new Tag() { TagName = "gold" }, new Tag() { TagName = "Egyption" } },
                Wants = new List<Tag> { new Tag() { TagName = "Revolver" }, new Tag() { TagName = "Makeup" }, new Tag() { TagName = "Necklaces" } },
                Value = "200",
                User = kari
            };
            var makeup = new Item()
            {
                Title = "Mac Makeup Kit",
                Description = "Lorem ipsum dolor Red Lipstick sit amet, Standard makeup kit consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "necklace1.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Mac" }, new Tag() { TagName = "Makeup" }, new Tag() { TagName = "lipstick" } },
                Wants = new List<Tag> { new Tag() { TagName = "eyeliner" }, new Tag() { TagName = "bronzer" }, new Tag() { TagName = "blush" } },
                Value = "70",
                User = snowman
            };
            var bbq = new Item()
            {
                Title = "Barbque",
                Description = "Lorem ipsum dolor barbeque sit amet, Standard  consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "barbeque.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "bbq" }, new Tag() { TagName = "Gas fueled" }, new Tag() { TagName = "500 degrees" } },
                Wants = new List<Tag> { new Tag() { TagName = "car jack" }, new Tag() { TagName = "rims" }, new Tag() { TagName = "tool kit" } },
                Value = "2000",
                User = kari
            };
            var lamp = new Item()
            {
                Title = "Ikea Lamp",
                Description = "Lorem Ikea Lamp 30volts dolor barbeque sit amet, Standard  consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "lamp2.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "lamp" } },
                Wants = new List<Tag> { new Tag() { TagName = "Household appliances" } },
                Value = "15",
                User = jack
            };
            var lamp2 = new Item()
            {
                Title = "Bed Bath and Beyond Lamp",
                Description = "Lorem Bed Bath and Beyond Lamp 50volts dolor barbeque sit amet, Standard  consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "lamp1.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "lamp" } },
                Wants = new List<Tag> { new Tag() { TagName = "Household appliances" } },
                Value = "15",
                User = snowman
            };
            //var glasses = new Item()
            //{
            //    Title = "Trendy Hipster Glasses",
            //    Description = "Lorem got them at H&M olor barbeque sit amet, Standard  consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
            //    Images = new List<Image> { new Image() { ItemImage = "glasses1.jpg" } },
            //    Status = ItemStatus.Available,
            //    Tags = new List<Tag> { new Tag() { TagName = "Glasses" }, new Tag() { TagName = "Trendy" } },
            //    Wants = new List<Tag> { new Tag() { TagName = "Scarf" } },
            //    Value = "30",
            //    User = admin
            //};
            //var computer = new Item()
            //{
            //    Title = "Dell Computer Desktop",
            //    Description = "Lorem got them at H&M olor barbeque sit amet, Standard  consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
            //    Images = new List<Image> { new Image() { ItemImage = "computer1.jpg" } },
            //    Status = ItemStatus.Available,
            //    Tags = new List<Tag> { new Tag() { TagName = "Desktop" }, new Tag() { TagName = "Monitors" } },
            //    Wants = new List<Tag> { new Tag() { TagName = "Video Games" } },
            //    Value = "1000",
            //    User = snowman
            //};
            var a = new Item()
            {
                Title = "Absolute",
                Description = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "absolute.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "hello world" }, new Tag() { TagName = "Revolver" }, new Tag() { TagName = "Silver" } },
                Wants = new List<Tag> { new Tag() { TagName = "cars" } },
                Value = "35000",
                User = admin
            };

            var b = new Item()
            {
                Title = "Lorem ipsum dolor si",
                Description = " elit. Quisque interdum odio eu lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "textbooks.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "elit world" }, new Tag() { TagName = "interdum" }, new Tag() { TagName = "pretium" } },
                Wants = new List<Tag> { new Tag() { TagName = "Integer" } },
                Value = "200",
                User = snowman
            };
            var c = new Item()
            {
                Title = "Cheedar Cheese Block",
                Description = "lectus luctus, ut gravida quam rutrum. Integer pretium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141031213132.png" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Cheese" }, new Tag() { TagName = "Block" }, new Tag() { TagName = "Cheedar" } },
                Wants = new List<Tag> { new Tag() { TagName = "pretium" } },
                Value = "200",
                User = kari
            };
            var d = new Item()
            {
                Title = "lectus luctus, ut gravida ",
                Description = "ium purus nunc, vel porttitor mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "scream.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "faucibus" }, new Tag() { TagName = "fermentum" }, new Tag() { TagName = "rhoncus" } },
                Wants = new List<Tag> { new Tag() { TagName = "cursus" } },
                Value = "700",
                User = jack
            };
            var e = new Item()
            {
                Title = "ium purus nunc, vel porttito ",
                Description = "r mauris faucibus et. Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141023215331.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "porttito" }, new Tag() { TagName = "purus" }, new Tag() { TagName = "nunc" } },
                Wants = new List<Tag> { new Tag() { TagName = "Maecenas" } },
                Value = "2050",
                User = admin
            };
            var f = new Item()
            {
                Title = "r mauris faucibus et.",
                Description = " Donec lacinia purus fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141025144326.png" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "rhoncus" }, new Tag() { TagName = "faucibus" }, new Tag() { TagName = "nunc" } },
                Wants = new List<Tag> { new Tag() { TagName = "Maecenas" } },
                Value = "5200",
                User = snowman
            };
            var g = new Item()
            {
                Title = "Donec lacinia purus",
                Description = "  fermentum felis luctus rhoncus. Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141028230659.png" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "lacinia" }, new Tag() { TagName = "faucibus" }, new Tag() { TagName = "purus" } },
                Wants = new List<Tag> { new Tag() { TagName = "Donec" } },
                Value = "52",
                User = jack
            };
            var h = new Item()
            {
                Title = "fermentum felis luctus rhoncus.",
                Description = " Maecenas at risus tristique, cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141031213133.png" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "fermentum" }, new Tag() { TagName = "rhoncus" }, new Tag() { TagName = "purus" } },
                Wants = new List<Tag> { new Tag() { TagName = "luctus" } },
                Value = "352",
                User = kari
            };
            var i = new Item()
            {
                Title = " Maecenas at risus tristique",
                Description = ", cursus risus non, finibus velit. Duis scelerisque neque massa, nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141109014318.gif" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "Maecenas" }, new Tag() { TagName = "risus" }, new Tag() { TagName = "tristique" } },
                Wants = new List<Tag> { new Tag() { TagName = "Vestibulum" } },
                Value = "852",
                User = admin
            };
            var j = new Item()
            {
                Title = "Duis scelerisque neque massa",
                Description = " nec sodales leo condimentum id. Vestibulum pharetra maximus congue. Vestibulum ipsum felis, fringilla quis ornare vitae, rutrum vel quam. Suspendisse nec maximus sem. Mauris nibh felis, ornare non lobortis nec, tincidunt eget justo. Cras fermentum quam eu mauris fermentum porttitor. Suspendisse porta at nibh a condimentum. Quisque vehicula interdum aliquam. Aenean volutpat nunc vel leo gravida ultrices. Donec viverra sem sed elit commodo, vel pretium risus aliquam. ",
                Images = new List<Image> { new Image() { ItemImage = "1_20141111180008.jpg" } },
                Status = ItemStatus.Available,
                Tags = new List<Tag> { new Tag() { TagName = "massa" }, new Tag() { TagName = "neque" }, new Tag() { TagName = "scelerisque" } },
                Wants = new List<Tag> { new Tag() { TagName = "Duis" } },
                Value = "5",
                User = snowman
            };




            context.Items.Add(fordChevy);
            context.Items.Add(shotgun);
            context.Items.Add(a);
            context.Items.Add(lamp2);
            context.Items.Add(b);
            context.Items.Add(redtruck2);
            context.Items.Add(c);
            context.Items.Add(necklace);
            context.Items.Add(d);
            context.Items.Add(makeup);
            context.Items.Add(e);
            context.Items.Add(bbq);
            context.Items.Add(f);
            context.Items.Add(redtruck1);
            context.Items.Add(g);
            context.Items.Add(lamp);
            context.Items.Add(h);
            //context.Items.Add(computer);
            context.Items.Add(i);
            context.Items.Add(redtruck3);
            context.Items.Add(j);
            //context.Items.Add(glasses);
            context.SaveChanges();

            //Messages
            var jackkariFirstMessage = new Message()
            {
                Reciever = kari,
                Sender = jack,
                MessageText = "What is the milage on your chevey?"
            };

            var karisentmessage = new Message()
            {
                Reciever = snowman,
                Sender = kari,
                MessageText = "How did you get a Fire Truck?"
            };

            var offerA = new Offer()
            {
                Sender = snowman,
                Reciever = kari,
                DesiredItems = new List<Item>{bbq},
                ProposedItems = new List<Item>{g},
                Messages = new List<Message>{new Message() {MessageText = "This bbq would be perfect for the summer, would you like to make a trade?"}},
                Status = OfferStatus.Initial
            };
            var offerb = new Offer()
            {
                Sender = admin,
                Reciever = kari,
                DesiredItems = new List<Item> { bbq },
                ProposedItems = new List<Item> { a, e },
                Messages = new List<Message> { new Message() { MessageText = "I will also give you 1000k for this bbq" } },
                Status = OfferStatus.Initial
            };
            var offerc = new Offer()
            {
                Sender = jack,
                Reciever = kari,
                DesiredItems = new List<Item> { bbq },
                ProposedItems = new List<Item> { lamp, shotgun },
                Messages = new List<Message> { new Message() { MessageText = "Shootgun has only been used once." } },
                Status = OfferStatus.Initial
            };
            //Offers set Items to "negotiation" as well
            var jackari = new Offer()
            {
                Sender = jack,
                Reciever = kari,
                DesiredItems = new List<Item>{ fordChevy },
                ProposedItems = new List<Item> { shotgun },
                Messages = new List<Message> { new Message() { MessageText = "Hi I want to trade you this shotgun for the car are you interested?" } },
                Status = OfferStatus.Initial
            };

            foreach (var message in jackari.Messages)
            {
                context.Messages.Add(message);
            }

            context.Messages.Add(jackkariFirstMessage);
            context.Messages.Add(karisentmessage);
            context.Offers.Add(jackari);
            context.Offers.Add(offerb);
            context.Offers.Add(offerA);
            context.Offers.Add(offerc);

            context.SaveChanges();

        }
    }
}