﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApiExamples.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class mike_testEntities : DbContext
    {
        public mike_testEntities()
            : base("name=mike_testEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<access> accesses { get; set; }
        public virtual DbSet<audiobook> audiobooks { get; set; }
        public virtual DbSet<datatables_demo> datatables_demo { get; set; }
        public virtual DbSet<dept> depts { get; set; }
        public virtual DbSet<file> files { get; set; }
        public virtual DbSet<site> sites { get; set; }
        public virtual DbSet<todo> todoes { get; set; }
        public virtual DbSet<user> users { get; set; }
    }
}
