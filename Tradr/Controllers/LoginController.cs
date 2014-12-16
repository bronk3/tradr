using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using Tradr.DAL;
using Tradr.Models;

namespace Tradr.Controllers
{
    public class LoginController : Controller
    {
        private TradrContext db = new TradrContext();
        private const int SALT_BYTE_SIZE = 24;
        private const int HASH_BYTE_SIZE = 24;
        private const int PBKDF2_ITERATIONS = 1000;

        private const int ITERATION_INDEX = 0;
        private const int SALT_INDEX = 1;
        private const int PBKDF2_INDEX = 2;

        public ActionResult Login()
        {
            this.Session["UserId"] = null;
            this.Session["UserName"] = null;
            return View(new ViewLogin());
        }

        [HttpPost]
        public ActionResult Login(ViewLogin model)
        {
            //Check pw & user
            var user = db.Users.SingleOrDefault(x => x.EmailAddress == model.EmailAddress);
            var password = user == null ? "" : user.Password;
            if (password != "" && ValidatePassword(model.Password, password))
            {
                this.Session["UserId"] = user.UserId;
                this.Session["UserName"] = user.FirstName;
                return Redirect(@Url.Action("Index", "Home"));
            }
            ViewBag.Errors = "True";
            return View(model);
        }

        public ActionResult NewUser()
        {
            return View(new User());
        }

        [HttpPost]
        public ActionResult NewUser(User model)
        {
            if (ModelState.IsValid)
            {
                model.Password = CreateHash(model.Password);
                db.Users.Add(model);
                db.SaveChanges();
                this.Session["UserId"] = model.UserId;
                this.Session["UserName"] = model.FirstName;
                return Redirect(@Url.Action("UserSettings", "User"));
            }
            ViewBag.Errors = "True";
            return View(model);
        }


        //https://crackstation.net/hashing-security.htm#aspsourcecode

        //Creates Hash with entered password to check against record hash
        public bool ValidatePassword(string password, string correctHash)
        {
            char[] delimiter = {':'};
            string[] split = correctHash.Split(delimiter);
            int iterations = Int32.Parse(split[ITERATION_INDEX]);
            byte[] salt = Convert.FromBase64String(split[SALT_INDEX]);
            byte[] hash = Convert.FromBase64String(split[PBKDF2_INDEX]);

            byte[] testHash = PBKDF2(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        //Prevents Hackers from being able to determine what Char is incorrect by having the run time all the same
        private bool SlowEquals(byte[] hash, byte[] testHash)
        {
            uint diff = (uint) hash.Length ^ (uint) testHash.Length;
            for (int i = 0; i < hash.Length && i < testHash.Length; i++)
                diff |= (uint) (hash[i] ^ testHash[i]);
            return diff == 0;
        }

        //Returns password Hash by using salt, password, and iterations
        public string CreateHash(string password)
        {
            RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            csprng.GetBytes(salt);

            byte[] hash = PBKDF2(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);
            return PBKDF2_ITERATIONS + ":" + Convert.ToBase64String(salt) + ":" + Convert.ToBase64String(hash);
        }

        //Hash function
        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations);
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
