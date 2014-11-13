using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            return View(user);
        }

    }
}
