using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Providers.Entities;
using Microsoft.Ajax.Utilities;
using Tradr.DAL;
using Tradr.Models;
using User = Tradr.Models.User;

namespace Tradr.Controllers
{
    public class HomeController : Controller
    {
        private TradrContext db = new TradrContext();

        public ActionResult Index(string search)
        {
            var userId = Convert.ToInt32(this.Session["UserId"]);
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var allItems = db.Items.Where(x => x.UserId != userId && x.Status != ItemStatus.Unavailable).ToList();
            if (search == null)
            {
                ViewBag.Header = "All Items";
                return View(allItems);
            }

            //In the future make a Dictionary ViewModel to segregate the Items
            //Based on:
            //Tags that match
            //Desired Item Tag Match
            //Title Match
            //Description Match
            var queryList = search.ToUpper().Split(new char[] {','});
            var subset = new List<Item>() {};
            foreach (var item in allItems)
            {
                var title = item.Title.ToUpper().Split(new char[] {' '});
                var Description = item.Description.ToUpper().Split(new char[] {' '});

                bool tagMatch = !queryList.Except(item.Tags.Select(y => y.TagName.ToUpper())).Any();
                bool titleMatch = !queryList.Except(title).Any();
                bool DescriptionMatch = !queryList.Except(Description).Any();

                if (tagMatch || titleMatch || DescriptionMatch) subset.Add(item);
            }
            ;
            if (subset.Count == 0) ViewBag.Header = "Unable to Find a Match for " + search;
            else ViewBag.Header = "Search Results For:" + search;
            return View(subset);
        }

        [HttpPost]
        public ActionResult Search()
        {
            return RedirectToAction("Index", "Home", new { search = Request.Form["mainSearch"] });
        }
    }
}
