﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FileFinder
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class FileFinderEntities : DbContext
    {
        public FileFinderEntities()
            : base("name=FileFinderEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Result> Results { get; set; }
        public virtual DbSet<Search> Searches { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
    }
}
