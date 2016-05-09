using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bibliothek.Controllers
{
    public class ReservierungController : Controller
    {
        //
        // GET: /Reservierung/

        
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult New(int buchId)
        {
            int deltaTage = 10;

            int nutzerId = (int)Session["user_Id"];

            Reservierung reservierung = new Reservierung();

            reservierung.NutzerId = nutzerId;
            reservierung.BuchId = buchId;

            Ausleihe ausleihe = null;
            using (var context = new bibliothekDBEntities())
            {
                ausleihe =
                    context.Ausleihe.FirstOrDefault(
                        x =>
                            (x.NutzerId == reservierung.NutzerId && x.BuchId == reservierung.BuchId &&
                             x.Rueckgabe == null));

                //1. nicht ausgeliehe
                if (ausleihe == null)
                {
                    reservierung.GueltigBis = DateTime.Today.AddDays(deltaTage);
                }
                //2. ausgeliehen
                else
                {
                    reservierung.GueltigBis = ausleihe.Bis.AddDays(deltaTage);
                }

                //Reservierung persistieren
                context.Reservierung.Add(reservierung);
                context.SaveChanges();
            }
            return View();
        }

    }
}
