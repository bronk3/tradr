using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls.Expressions;
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
            var desiredId = Convert.ToInt32(Request.Form["desiredId"]);
            var senderId = Convert.ToInt32(Request.Form["senderId"]);
            var recieverId = Convert.ToInt32(Request.Form["recieverId"]);

            if (desiredId != 0 && senderId != 0 && recieverId != 0)
            {
                var sender = db.Users.FirstOrDefault(x => x.UserId == senderId);
                var receiver = db.Users.FirstOrDefault(x => x.UserId == recieverId);

                model.RecieverId = recieverId;
                model.SenderId = senderId;
                model.Reciever = receiver;
                model.Sender = sender;

                AddDesiredItems(Request.Form["desiredItems"], model);
                AddProposedItems(Request.Form["items"], model);
                UpdateItemStatus(model.ProposedItems.ToList());
                UpdateItemStatus(model.DesiredItems.ToList());
                model.Messages.Add(CreateMessage(Request.Form["message"], model));
                model.Status = OfferStatus.Initial;
                model.DateTimeInitial = DateTime.UtcNow;
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
        public ActionResult ViewOffer(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            return View(offer);
        }
        public ActionResult NegotiateOffer(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            offer.See = OfferSee.Seen;
            db.SaveChanges();
            return View(offer);
        }
        ///THIS IS BROKEN BECAUSE THE OFFER BEING SENT BACK IS GETTING WHIPED CLLEAN OF ITS 
        /// DESIRED ITEM ID - FIX IT SO IT EITHER GETS SET IN NTHE VIEW OR GETS SET IN THE CONTROLLER
        [HttpPost]
        public ActionResult NegotiateOffer()
        {
            var formId = Convert.ToInt32(Request.Form["offerId"]);
            var model = db.Offers.SingleOrDefault(x => x.OfferId == formId);
            model.DesiredItems.Add(db.Items.SingleOrDefault(x => x.ItemId == formId));
            AddProposedItems(Request.Form["items"], model);
            model.Messages.Add(CreateMessage(Request.Form["message"], model));
            model.Status = OfferStatus.Negotiation;
            db.SaveChanges();
            return RedirectToAction("Outbox");
        }
        public ActionResult Outbox()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var outbox =
                user.SentOffers.Where(x => x.Status != OfferStatus.Accepted && x.Status != OfferStatus.Rejected).ToList();
            var negotiated = user.RecievedOffers.Where(x => x.Status == OfferStatus.Negotiation).ToList();
            var combined = outbox.Concat(negotiated).ToList();
            var errorFree = combined.Where(x => x.DesiredItems.Count != 0).ToList();
            return View(errorFree);
        }

        public ActionResult Inbox()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var inbox =
                user.RecievedOffers.Where(x => x.Status != OfferStatus.Accepted && x.Status != OfferStatus.Rejected).ToList();
            var negotiations = user.SentOffers.Where(x => x.Status == OfferStatus.Negotiation).ToList();
            var combined = inbox.Concat(negotiations).ToList();
            var errorFree = combined.Where(x => x.DesiredItems.Count != 0).ToList();
                return View(errorFree);
        }

        public ActionResult History()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            ViewBag.UserId = userId;
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var History =
                user.RecievedOffers.Where(x => x.Status == OfferStatus.Accepted || x.Status == OfferStatus.Rejected).ToList();
            var history2 = user.SentOffers.Where(x => x.Status == OfferStatus.Accepted || x.Status == OfferStatus.Rejected).ToList();
            var combined = History.Concat(history2).ToList();
            var errorFree = combined.Where(x => x.DesiredItems.Count != 0).ToList();
            return View(errorFree);
        }
        public ActionResult RemoveOffer(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            var messages = offer.Messages.ToList();
            messages.ForEach(x => db.Messages.Remove(x));
            db.Offers.Remove(offer);
            db.SaveChanges();
            return RedirectToAction("Outbox", "Offer");
        }

        public ActionResult RejectOffer(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            offer.Status = OfferStatus.Rejected;
            var value = offer.Status;
            //Assuming they were only in one offer at the time
            //I think offer status of "Negotiation" should be removed
            //And replaced with a count of how many offers the item is in
            itemsRejected(offer.DesiredItems.ToList());
            itemsRejected(offer.ProposedItems.ToList());
            db.SaveChanges();
            return RedirectToAction("History", "Offer");
        }

        public ActionResult AcceptOffer(int id)
        {
            var offer = db.Offers.SingleOrDefault(x => x.OfferId == id);
            offer.Status = OfferStatus.Accepted;
            itemsAccepted(offer.DesiredItems.ToList());
            itemsAccepted(offer.ProposedItems.ToList());
            db.SaveChanges();
            return RedirectToAction("History", "Offer");
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
                DateTimeMessage = DateTime.UtcNow,
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
    }
}
