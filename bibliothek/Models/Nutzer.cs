using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace bibliothek
{
    public partial class Nutzer
    {
        public List<Ausleihe> AusleiheList
        {
            get
            {
                List<Ausleihe> ausleiheList = new List<Ausleihe>();

                using (var context = new bibliothekDBEntities())
                {
                    ausleiheList = context.Ausleihe.Where(x => (x.NutzerId == this.Id && x.Rueckgabe.Value == null)).ToList();

                    foreach (Ausleihe ausleihe in ausleiheList)
                    {
                        ausleihe.Buch = context.Buch.FirstOrDefault(buch => buch.Id == ausleihe.BuchId);
                    }
                }
                return ausleiheList;
            }
        }

    }
}