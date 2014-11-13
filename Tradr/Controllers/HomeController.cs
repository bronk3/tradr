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
        

        public ActionResult Index()
        {
            var userId = Convert.ToInt32(this.Session["UserId"]);
            var user = db.Users.SingleOrDefault(x => x.UserId == userId);
            var allItems = db.Items.Where(x => x.UserId != userId).ToList();
            return View(allItems);
        }
    }
}
