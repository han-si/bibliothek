using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bibliothek.Controllers
{
    public class BuchController : Controller
    {
 
        public ActionResult Index(string msg = "")
        {
            List<Buch> list = GetBuchList(); 
            
            ViewBag.Msg = msg;

            return View(list);
        }

        [HttpGet]
        public ActionResult New()
        {
            Buch buch = new Buch();

            return View(buch);
        }

        [HttpPost]
        public ActionResult New(Buch buch)
        {
            if (ModelState.IsValid)
            {
                using (var context = new bibliothekDBEntities())
                {
                    context.Buch.Add(buch);
                    context.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(buch);
        }

        public ActionResult Delete(int id)
        {
            string message = "";

            using (var context = new bibliothekDBEntities())
            {
                Buch buch = context.Buch.FirstOrDefault(x => x.Id == id);
                if (buch != null)
                {
                    context.Buch.Remove(buch);
                    context.SaveChanges();
                    message = "Löschen war erfolgreich";
                }
                else
                {
                    message = "Löschen fehlgeschlagen";
                }
            }

            return RedirectToAction("Index", new {msg = message});

        }

        private static List<Buch> GetBuchList()
        {
            List<Buch> list = null;

            using (var context = new bibliothekDBEntities())
            {
                list = context.Buch.ToList();
            }

            return list; 
        }
    }
}
