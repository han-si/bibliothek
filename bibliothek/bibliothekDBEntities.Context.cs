﻿//------------------------------------------------------------------------------
// <auto-generated>
//    Dieser Code wurde aus einer Vorlage generiert.
//
//    Manuelle Änderungen an dieser Datei führen möglicherweise zu unerwartetem Verhalten Ihrer Anwendung.
//    Manuelle Änderungen an dieser Datei werden überschrieben, wenn der Code neu generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace bibliothek
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class bibliothekDBEntities : DbContext
    {
        public bibliothekDBEntities()
            : base("name=bibliothekDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Ausleihe> Ausleihe { get; set; }
        public DbSet<Buch> Buch { get; set; }
        public DbSet<Nutzer> Nutzer { get; set; }
        public DbSet<Reservierung> Reservierung { get; set; }
    }
}
