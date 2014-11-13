using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Antlr.Runtime.Tree;
using Newtonsoft.Json.Converters;
using Tradr.DAL;
using Tradr.Models;
using WebGrease.Css.Extensions;

namespace Tradr.Controllers
{
    public class ItemController : Controller
    {
        private TradrContext db = new TradrContext();

        public ActionResult NewItem()
        {
            return View(new ViewItem());
        }

        public List<Tag> AddTags(string list, string type, Item item, User user)
        {
            var tagList = new List<Tag>();
            if (list != null)
            {
                foreach (var tagString in list.Split(new char[] { ',' }, StringSplitOptions.None))
                {
                    var tagObject = db.Tags.SingleOrDefault(x => x.TagName == tagString);
                    tagObject = (tagObject) ?? new Tag(){TagName = tagString}; 
                    if (type == "wants") tagObject.ItemWantsTags.Add(item);
                    else if (type == "tags") tagObject.ItemTagsTags.Add(item);
                    else if (type == "userWants") tagObject.Users.Add(user);
                    db.Tags.Add(tagObject);
                    db.SaveChanges();
                    tagList.Add(tagObject);
                }
            }
            return tagList;
        }

        public List<Image> AddImages(string list)
        {
            var imageList = new List<Image>();
            if(list != null){
            list.Split(new char[] {','}, StringSplitOptions.None)
                    .ForEach(x => imageList.Add(new Image() {ItemImage = x}));
            }
            return imageList;
        }

        [HttpPost]
        public string ImageUpload(HttpPostedFileBase file)
        {
            var autoGen = "";
            if (file != null && file.ContentLength > 0 && file.FileName != null)
            {
                var userId = 1;
                autoGen = string.Format("{0}_{1}{2}", userId, DateTime.UtcNow.ToString("yyyyMMddHHmmss"), System.IO.Path.GetExtension(file.FileName));
                string path = System.IO.Path.Combine(Server.MapPath("~/Content/images/items"),
                    autoGen);
                file.SaveAs(path);
            }
            return autoGen;
        }

        public ActionResult ItemDetails(int id)
        {
            var item = db.Items.SingleOrDefault(x => x.ItemId == id);
            item.Views= item.Views + 1;
            db.SaveChanges();
            
            return View(item);
        }

        [HttpPost]
        public ActionResult NewItem(ViewItem model)
        {
            // Db Model Item
            var newItem = new Item();
            newItem.UserId = Convert.ToInt32(this.Session["UserId"]);
            newItem.User = db.Users.SingleOrDefault(x => x.UserId == newItem.UserId);
            db.Items.Add(newItem);
            db.SaveChanges();

            //ViewItem Values
            newItem.Title = model.Title;
            newItem.Value = model.Value;
            newItem.Description = model.Description;
            newItem.Tags = AddTags(model.Tags, "tags", newItem, null);
            newItem.Wants = AddTags(model.Wants, "wants", newItem, null);
            newItem.Images = AddImages(model.Images);
            db.SaveChanges();
            return Redirect(Url.Action("MyItems", "Item"));
        }

        public ActionResult EditItem(int id)
        {
            var userId = Convert.ToInt32(this.Session["UserId"]);
            var item = db.Items.SingleOrDefault(x => x.ItemId == id && x.UserId == userId);
            if (item == null)
            {
                return Redirect(Url.Action("Warning", "Item", new {id = id}));
            }
            else
            {
                ViewBag.Tags = String.Join(",", item.Tags.Select(x=> x.TagName));
                ViewBag.Wants = String.Join(",", item.Wants.Select(x => x.TagName));
                return View(item);
            }
        }

        [HttpPost]
        public ActionResult EditItem(Item model)
        {
            if (ModelState.IsValid)
            {
                db.SaveChanges();
            return Redirect(Url.Action("MyItems", "Item"));
        }
            return View(model);
        }

        public ActionResult MyItems()
        {
            var userId = Convert.ToInt32(this.Session["UserId"]);
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            if (user == null)
            {
                return RedirectToAction("Login", "Login");
            }
            var items = user.Items.ToList();
            return View(items);
        }

        public ActionResult Warning(int id)
        {
            return View(id);
        }

        public ActionResult RemoveItem(int id)
        {
            var item = db.Items.SingleOrDefault(x => x.ItemId == id);
            var itemImages = item.Images.ToList();
            var imageList = db.Images.Where(x => db.Items.FirstOrDefault(z => z.ItemId == id).Images.Any(y => x.ImageId == y.ImageId)).ToList();
            imageList.ForEach(x => db.Images.Remove(x));
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("MyItems", "Item");
        }

    }
}
