using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls.Expressions;
using Microsoft.Ajax.Utilities;
using Tradr.DAL;
using Tradr.Models;
using WebGrease.Css.Extensions;

namespace Tradr.Controllers
{
    public class OfferController : Controller
    {
        private TradrContext db = new TradrContext();

        public ActionResult MakeAnOffer(int id)
        {
            var offer = new Offer();
            var item = db.Items.SingleOrDefault(x => x.ItemId == id);
            offer.DesiredItems.Add(item);

            var recieverId = item.UserId;
            offer.RecieverId = recieverId;

            var reciever = item.User;
            offer.Reciever = reciever;

            var currentUser = Convert.ToInt32(this.Session["UserId"]);
            offer.SenderId = currentUser;
            offer.Sender = db.Users.SingleOrDefault(x => x.UserId == currentUser);

            return View(offer);
        }

        [HttpPost]
        public ActionResult MakeAnOffer(Offer model)
        {
            var senderId = Convert.ToInt32(Request.Form["senderId"]);
            var recieverId = Convert.ToInt32(Request.Form["recieverId"]);

            if (senderId != 0 && recieverId != 0)
            {
                var sender = db.Users.FirstOrDefault(x => x.UserId == senderId);
                var receiver = db.Users.FirstOrDefault(x => x.UserId == recieverId);

                model.RecieverId = recieverId;
                model.SenderId = senderId;
                model.Reciever = receiver;
                model.Sender = sender;

                AddDesiredItems(Request.Form["desiredItems"], model);
                AddProposedItems(Request.Form["Offereditems"], model);
                UpdateItemStatus(model.ProposedItems.ToList());
                UpdateItemStatus(model.DesiredItems.ToList());
                model.Messages.Add(CreateMessage(Request.Form["message"], model));
                model.Status = OfferStatus.Initial;
                model.DateTime = DateTime.UtcNow;
                model.See = OfferSee.NotSeen;

                db.Offers.Add(model);
                db.SaveChanges();
                return Redirect(@Url.Action("Outbox", "Offer"));
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult ViewOffer(int id, string previous)
        {
            ViewBag.curUser = Convert.ToInt32(this.Session["UserId"]);
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            if (offer == null)
            {
               return RedirectToAction("Warning", "Offer");
            }
            if (!string.IsNullOrEmpty(previous))
            {
                ViewBag.Previous = "true";
            }
            return View(offer);
        }
        public ActionResult NegotiateOffer(int id, OfferStatus? negotiate)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            if (offer == null)
            {
               return RedirectToAction("Warning", "Offer");
            }
            offer.See = OfferSee.Seen;
            db.SaveChanges();
            if (negotiate != null && negotiate == OfferStatus.Negotiation)
            {
                ViewBag.Negotiation = OfferStatus.Negotiation;
            }
            return View(offer);
        }
        [HttpPost]
        public ActionResult NegotiateOffer(Offer model)
        {
            var senderId = Convert.ToInt32(Request.Form["senderId"]);
            var recieverId = Convert.ToInt32(Request.Form["recieverId"]);

            if (senderId != 0 && recieverId != 0)
            {
                var sender = db.Users.FirstOrDefault(x => x.UserId == senderId);
                var receiver = db.Users.FirstOrDefault(x => x.UserId == recieverId);

                model.RecieverId = recieverId;
                model.SenderId = senderId;
                model.Reciever = receiver;
                model.Sender = sender;

                AddDesiredItems(Request.Form["desiredItems"], model);
                AddProposedItems(Request.Form["Offereditems"], model);
                UpdateItemStatus(model.ProposedItems.ToList());
                UpdateItemStatus(model.DesiredItems.ToList());
                model.Messages.Add(CreateMessage(Request.Form["message"], model));
                model.Status = OfferStatus.Initial;
                model.DateTime = DateTime.UtcNow;
                model.See = OfferSee.NotSeen;

                //Dealing Now with Linking Offers
                var previousOfferId = Convert.ToInt32(Request.Form["oldOfferid"]);
                model.PreviousOffer = previousOfferId;
                var prevOffer = db.Offers.FirstOrDefault(x => x.OfferId == previousOfferId);
                
                db.Offers.Add(model);
                db.SaveChanges();

                prevOffer.NextOffer = model.OfferId;
                prevOffer.Status = OfferStatus.Negotiation;

                db.SaveChanges();

                return Redirect(@Url.Action("Outbox", "Offer"));
            }
            else
            {
                return View(model);
            }
        }
        public ActionResult Outbox()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            if (userId == 0) return Redirect(Url.Action("Login", "Login"));
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var outbox =
                user.SentOffers.Where(x => x.Status != OfferStatus.Accepted && x.Status != OfferStatus.Rejected && x.Status != OfferStatus.Cancelled).ToList();
            var errorFree = outbox.Where(x => x.DesiredItems.Count != 0).ToList();
            var messages =
                db.Messages.Where(
                    x => x.Sender.UserId == user.UserId && x.Offer == null && x.Status != MessageStatus.History)
                    .ToList();
            var view = new List<IdateObject>();
            view.AddRange(errorFree);
            view.AddRange(messages);
          
            return View(view);
        }

        public ActionResult Inbox()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            if (userId == 0) return Redirect(Url.Action("Login", "Login"));
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var inbox =
                user.RecievedOffers.Where(x => x.Status != OfferStatus.Accepted && x.Status != OfferStatus.Rejected && x.Status != OfferStatus.Cancelled).ToList();
            var errorFree = inbox.Where(x => x.DesiredItems.Count != 0).ToList();
            var messages =
                db.Messages.Where(
                    x => x.Reciever.UserId == user.UserId && x.Offer == null && x.Status != MessageStatus.History)
                    .ToList();
            var view = new List<IdateObject>();
            view.AddRange(errorFree);
            view.AddRange(messages);

            //var boxview = new BoxView()
            //{
            //    Messages = messages,
            //    Offers = errorFree
                
            //};

                return View(view);
        }

        public ActionResult History()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            if (userId == 0) return Redirect(Url.Action("Login", "Login"));
            ViewBag.UserId = userId;
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var History =
                user.RecievedOffers.Where(x => x.Status == OfferStatus.Accepted || x.Status == OfferStatus.Rejected || x.Status == OfferStatus.Cancelled).ToList();
            var history2 = user.SentOffers.Where(x => x.Status == OfferStatus.Accepted || x.Status == OfferStatus.Rejected || x.Status == OfferStatus.Cancelled).ToList();
            var combined = History.Concat(history2).ToList();
            var errorFree = combined.Where(x => x.DesiredItems.Count != 0 && x.NextOffer == 0).ToList();
            var messages =
                db.Messages.Where(
                    x =>
                        (x.Reciever.UserId == user.UserId || x.Sender.UserId == user.UserId) && x.Offer == null &&
                        x.Status == MessageStatus.History).ToList();
            var view = new List<IdateObject>();
            view.AddRange(errorFree);
            view.AddRange(messages);

            return View(view);
        }
        public ActionResult RemoveOffer(int id)
        {
            RemovePreviousOffers(id);
            return RedirectToAction("Outbox", "Offer");
        }

        public void RemovePreviousOffers(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            if (offer == null)
            {
                return;
            }
            var messages = offer.Messages.ToList();
            messages.ForEach(x => db.Messages.Remove(x));
            if (offer.PreviousOffer != 0)
            {
                RemovePreviousOffers(offer.PreviousOffer);
            }
            db.Offers.Remove(offer);
            db.SaveChanges();
        }

        public ActionResult RejectOffer(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            if (offer == null)
            {
                return RedirectToAction("Warning", "Offer");
            }
            offer.Status = OfferStatus.Rejected;
            itemsRejected(offer.DesiredItems.ToList());
            itemsRejected(offer.ProposedItems.ToList());
            if (offer.PreviousOffer != 0)
            {
                RemovePreviousOffers(offer.PreviousOffer);
            }
            db.SaveChanges();
            return RedirectToAction("History", "Offer");
        }

        public ActionResult AcceptOffer(int id)
        {
            //Remove other offers 
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            if (offer == null)
            {
               return RedirectToAction("Warning", "Offer");
            }
            itemsAccepted(offer.DesiredItems.ToList());
            itemsAccepted(offer.ProposedItems.ToList());
            CancelOffers(offer.DesiredItems.ToList());
            CancelOffers(offer.ProposedItems.ToList());
            if (offer.PreviousOffer != 0)
            {
                RemovePreviousOffers(offer.PreviousOffer);
            }
            offer.Status = OfferStatus.Accepted;
            db.SaveChanges();
            return RedirectToAction("History", "Offer");
        }

        public void CancelOffers(IEnumerable<Item> items)
        {   
            var user = items.First().User;
            foreach (var item in items)
            {
                user.RecievedOffers.Where(x => x.DesiredItems.Any(y => y.ItemId == item.ItemId && x.Status != OfferStatus.Rejected)).ForEach(z => z.Status = OfferStatus.Cancelled);
                user.SentOffers.Where(x => x.ProposedItems.Any(y => y.ItemId == item.ItemId && x.Status != OfferStatus.Rejected)).ForEach(z => z.Status = OfferStatus.Cancelled);
            }
        }
        public void AddProposedItems(string items, Offer model)
        {
            //For Negotiations
            model.ProposedItems.Clear();
            var array = items.Split(new char[] { ',' });
            foreach (var id in array)
            {
                var intid = Convert.ToInt32(id);
                var item = db.Items.SingleOrDefault(x => x.ItemId == intid);
                model.ProposedItems.Add(item);
            }
        }
        public void AddDesiredItems(string items, Offer model)
        {
            //For Negotiations
            model.DesiredItems.Clear();
            var array = items.Split(new char[] { ',' });
            foreach (var id in array)
            {
                var intid = Convert.ToInt32(id);
                var item = db.Items.SingleOrDefault(x => x.ItemId == intid);
                model.DesiredItems.Add(item);
            }
        }
        public Message CreateMessage(string message, Offer model)
        {
            var newMessage = new Message()
            {
                DateTime = DateTime.UtcNow,
                MessageText = message,
                Reciever = model.Reciever,
                Sender = model.Sender
            };
            db.Messages.Add(newMessage);
            db.SaveChanges();
            return newMessage;
        }
        public void UpdateItemStatus(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                item.Status = ItemStatus.Negotiation;
            }
        }
        public void itemsAccepted(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                item.Status = ItemStatus.Unavailable;
            }
        }

        public void itemsRejected(IEnumerable<Item> items)
        {
            foreach (var item in items)
            {
                item.Status = ItemStatus.Available;
            }
        }

        public ActionResult Warning()
        {
            return View();
        }
    }
}
