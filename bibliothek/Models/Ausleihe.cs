using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace bibliothek
{
    public partial class Ausleihe
    {
        [NotMapped]
        public Nutzer Nutzer { get; set; }

        [NotMapped]
        public Buch Buch { get; set; }

        [NotMapped]
        public IEnumerable<SelectListItem> NutzerList { get; set; }

        public Ausleihe()
        {
            NutzerList = GetNutzerSelectList();
        }

        private List<SelectListItem> GetNutzerSelectList()
        {
            List<Nutzer> nutzerList = null;
            using (var context = new bibliothekDBEntities())
            {
                nutzerList = context.Nutzer.ToList();
            }

            //Zielliste
            var items = new List<SelectListItem>();

            foreach (Nutzer nutzer in nutzerList)
            {
                var item = new SelectListItem();
                item.Value = nutzer.Id.ToString();
                item.Text = nutzer.Name + ", " +  nutzer.Vorname;
                items.Add(item);
            }
            return items;
        }
    }
}