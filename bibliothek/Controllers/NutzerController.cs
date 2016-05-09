using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bibliothek.Controllers
{
    [Authorize]
    public class NutzerController : Controller
    {

        [Authorize(Users = "Admin")]
        public ActionResult Index(string msg = "")
        {
            List<Nutzer> list = GetNutzerList();

            ViewBag.Msg = msg;

            return View(list);
        }

        [Authorize(Users = "Admin")]
        [HttpGet]
        public ActionResult New()
        {
            Nutzer nutzer = new Nutzer();
            nutzer.Geburtsdatum = DateTime.Now;

            return View(nutzer);
        }

        [Authorize(Users = "Admin")]
        [HttpPost]
        public ActionResult New(Nutzer nutzer)
        {
            if (ModelState.IsValid)
            {
                nutzer.IstAktiv = false;
                
                using (var context = new bibliothekDBEntities())
                {
                    context.Nutzer.Add(nutzer);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(nutzer);

        }

        [Authorize(Users = "Admin")]
        public ActionResult Delete(int id)
        {
            string message = "";

            using (var context = new bibliothekDBEntities())
            {
                Nutzer nutzer = context.Nutzer.FirstOrDefault(x => x.Id == id);
                if (nutzer != null)
                {
                    context.Nutzer.Remove(nutzer);
                    context.SaveChanges();
                    message = "Löschen war erfolgreich";
                }
                else
                {
                    message = "Löschen fehlgeschlagen";
                }
            }

            return RedirectToAction("Index", new { msg = message });
        }


        public ActionResult AusleiheOverview(int nutzerId)
        {
            //Nutzer (bzw. nicht Admin) darf fremde Ausleihe nicht sehen
            if (!@Session["user_Login"].ToString().Equals("Admin") && !@Session["User_Id"].ToString().Equals(nutzerId.ToString()))
            {
                return RedirectToAction("NotFound", "Home");
            }
            Nutzer nutzer = null;

            using (var context = new bibliothekDBEntities())
            {
                nutzer = context.Nutzer.FirstOrDefault(x => x.Id == nutzerId);

                if (nutzer != null) return View(nutzer);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Users = "Admin")]
        private static List<Nutzer> GetNutzerList()
        {
            List<Nutzer> list = null;

            using (var context = new bibliothekDBEntities())
            {
                list = context.Nutzer.ToList();
            }

            return list;
        }


        [Authorize(Users = "Admin")]
        private static void GenerateNutzerList()
        {

            Nutzer nutzer1 = new Nutzer{Name = "Kluge", Vorname = "Hansi", Geburtsdatum = new DateTime(1968, 01, 01), Login = "Hansi", Passwort = "123", IstAktiv = false};
            Nutzer nutzer2 = new Nutzer {Name = "Mustermann", Vorname = "Max", Geburtsdatum = new DateTime(1968, 02, 01), Login = "Max", Passwort = "123", IstAktiv = false};
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
                var all = context.Nutzer;
                context.Nutzer.RemoveRange(all);
                context.SaveChanges();
            }

            GenerateNutzerList();

            return RedirectToAction("Index");
        }

        [Authorize(Users = "Admin")]
        public ActionResult Return(int ausleiheId, int nutzerId)
        {
            using (var context = new bibliothekDBEntities())
            {
                Ausleihe ausleihe = context.Ausleihe.FirstOrDefault(x => x.Id == ausleiheId);
                if (ausleihe != null)
                {
                    ausleihe.Rueckgabe = DateTime.Today;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("AusleiheOverview" , new {nutzerId = nutzerId});
        }


    }
}
