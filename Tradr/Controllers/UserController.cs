using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Tradr.DAL;
using Tradr.Models;

namespace Tradr.Controllers
{
    public class UserController : Controller
    {
        private TradrContext db = new TradrContext();

        public ActionResult UserSettings()
        {
            var userId = Convert.ToInt32(this.Session["UserId"]);
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var wantsString = sendTags(user.Wants);
            ViewBag.Wants = wantsString;
            ViewBag.Saved = TempData["Saved"];
            return View(user);
        }

        [HttpPost]
        public ActionResult UserSettings(Tradr.Models.User model)
        {
                var userId = Convert.ToInt32(this.Session["UserId"]);
                var user = db.Users.SingleOrDefault(x => x.UserId == userId);
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.EmailAddress = model.EmailAddress;
                user.SecondaryEmail = model.SecondaryEmail;
                user.Phone = model.Phone;
                user.Street = model.Street;
                user.AreaCode = model.AreaCode;
                AddTags(Request.Form["Wants"], user);
                db.SaveChanges();

                TempData["Saved"] = "True";
                return RedirectToAction("UserSettings");
        }

        public ActionResult Profile(int id)
        {
            var user = db.Users.SingleOrDefault(x => x.UserId == id);
            return View(user);
        }

        //This does create a message in the database
        //The issue is displaying it in the outbox/inbox
        //Will have to make a new outbox/Inbox View to be able display
        // Messages as well as offers.
        public ActionResult Message(int id, int? previous, int? item)
        {
            var userId = Convert.ToInt32(this.Session["UserId"]);
            var reciever = db.Users.FirstOrDefault(x => x.UserId == id);
            ViewBag.Reciever = reciever.FirstName;
            var message = new MessageView()
            {
                PreviousMessage = db.Messages.FirstOrDefault(x => x.MessageId == previous),
                Item = db.Items.FirstOrDefault(x=> x.ItemId == item),
                SenderId = userId,
                RecieverId = id
            };
            return View(message);
        }

        [HttpPost]
        public ActionResult Message(MessageView model)
        {
            var message = new Message()
            {
                Sender = db.Users.SingleOrDefault(x => x.UserId == model.SenderId),
                Reciever = db.Users.SingleOrDefault(x => x.UserId == model.RecieverId),
                MessageText = model.Message
            };
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("Outbox", "Offer");
        }

        public ActionResult ArchiveMessage(int id)
        {
            var message = db.Messages.FirstOrDefault(x => x.MessageId == id);
            if (message != null)
            {
                message.Status = MessageStatus.History;
            }
            db.SaveChanges();
            return RedirectToAction("History", "Offer");
        }

        public string sendTags(IEnumerable<Tag> wants)
        {
            return String.Join(", ", wants.Select(x => x.TagName));
        }

        public void AddTags(string list, Tradr.Models.User user)
        {
            user.Wants.Clear();
            var tagList = new List<Tag>();
            if (list != null)
            {
                foreach (var tagString in list.Split(new char[] { ',' }, StringSplitOptions.None))
                {
                    var tagObject = db.Tags.FirstOrDefault(x => x.TagName.ToLower() == tagString.ToLower());
                    tagObject = (tagObject) ?? new Tag() { TagName = tagString };
                    user.Wants.Add(tagObject);
                    db.Tags.Add(tagObject);
                    db.SaveChanges();
                    tagList.Add(tagObject);
                }
            }
        }

    }
}
