using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using bibliothek.Models;

namespace bibliothek.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            if (@Session["user_Id"] == null)
            {
                FormsAuthentication.SignOut();
            }

            if (!Request.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            LoginModel loginModel = new LoginModel();
            
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            string login = model.Login;
            string passwort = model.Passwort;

            Nutzer nutzer = null;

            using (var context = new bibliothekDBEntities())
            {
                nutzer = context.Nutzer.FirstOrDefault(x => (x.Login == login && x.Passwort == passwort));
            }

            if (nutzer == null)
            {
                model = new LoginModel();
                ViewBag.Msg = "Login und/oder Passwort falsch";

                return View(model);
            }

            //Logindaten korrekt, dann speichere Nutzerdaten in Session
            FormsAuthentication.SetAuthCookie(nutzer.Login, false);

            Session["user_Login"] = nutzer.Login;
            Session["user_Id"] = nutzer.Id;

            //später ändern
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login");
        }

        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View();
        }

        private static void GenerateBuchList()
        {

            Buch buch1 = new Buch { Titel = "Faust", Autor = "Goethe", ISBN = "123-456" };
            Buch buch2 = new Buch { Titel = "Windows 10", Autor = "Bill Gates", ISBN = "123-457" };
            Buch buch3 = new Buch { Titel = "BlackBook", Autor = "Ingo Hacker", ISBN = "455-8123-1" };

            //Zugriff auf DB-Context
            using (var context = new bibliothekDBEntities())
            {
                context.Buch.Add(buch1);
                context.Buch.Add(buch2);
                context.Buch.Add(buch3);

                context.SaveChanges();
            }
        }

        private static void GenerateNutzerList()
        {

            Nutzer nutzer1 = new Nutzer { Name = "Kluge", Vorname = "Hansi", Geburtsdatum = new DateTime(1968, 01, 01), Login = "Hansi", Passwort = "123", IstAktiv = false };
            Nutzer nutzer2 = new Nutzer { Name = "Mustermann", Vorname = "Max", Geburtsdatum = new DateTime(1968, 02, 01), Login = "Max", Passwort = "123", IstAktiv = false };
            Nutzer nutzer3 = new Nutzer { Name = "Nutzer", Vorname = "Admin", Geburtsdatum = new DateTime(1999, 04, 11), Login = "Admin", Passwort = "123", IstAktiv = false };

            //Zugriff auf DB-Context
            using (var context = new bibliothekDBEntities())
            {
                context.Nutzer.Add(nutzer1);
                context.Nutzer.Add(nutzer2);
                context.Nutzer.Add(nutzer3);

                context.SaveChanges();
            }
        }

        [Authorize(Users = "Admin")]
        public ActionResult Reset()
        {
            using (var context = new bibliothekDBEntities())
            {
                var allNutzer = context.Nutzer;
                context.Nutzer.RemoveRange(allNutzer);

                var allBuch = context.Buch;
                context.Buch.RemoveRange(allBuch);

                var allAusleihe = context.Ausleihe;
                context.Ausleihe.RemoveRange(allAusleihe);

                context.SaveChanges();
            }

            GenerateBuchList();
            GenerateNutzerList();

            return RedirectToAction("Index");
        }

    }
}
