using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bibliothek.Controllers
{
    [Authorize(Users = "Admin")]
    public class AusleiheController : Controller
    {
        public ActionResult Index()
        {
            List<Ausleihe> list = GetAusleiheList();

            return View(list);
        }

        [HttpGet]
        public ActionResult New(int buchId)
        {
            Ausleihe ausleihe = new Ausleihe();

            using (var context = new bibliothekDBEntities())
            {
                ausleihe.Buch = context.Buch.FirstOrDefault(x => x.Id == buchId);
            }

            ausleihe.Von = DateTime.Today;
            ausleihe.Bis = DateTime.Today.AddDays(30);

            return View(ausleihe);
        }

        [HttpPost]
        public ActionResult New(Ausleihe ausleihe)
        {
            if (ModelState.IsValid)
            {
                using (var context = new bibliothekDBEntities() )
                {
                    context.Ausleihe.Add(ausleihe);
                    context.SaveChanges();
                }
                
                return RedirectToAction("Index");
            }
            return View(ausleihe);
        }

        private static List<Ausleihe> GetAusleiheList()
        {
            List<Ausleihe> list = null;

            using (var context = new bibliothekDBEntities())
            {
                list = context.Ausleihe.ToList();

                foreach (Ausleihe ausleihe in list)
                {
                    ausleihe.Buch = context.Buch.FirstOrDefault(x => x.Id == ausleihe.BuchId);
                    ausleihe.Nutzer = context.Nutzer.FirstOrDefault(x => x.Id == ausleihe.NutzerId);
                }
            }
            return list;
        }

        public ActionResult Return(int id)
        {
            using (var context = new bibliothekDBEntities())
            {
                Ausleihe ausleihe = context.Ausleihe.FirstOrDefault(x => x.Id == id);
                if (ausleihe != null)
                {
                    ausleihe.Rueckgabe = DateTime.Today;
                    context.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
