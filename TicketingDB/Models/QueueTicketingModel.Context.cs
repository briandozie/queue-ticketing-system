﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TicketingDB.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TicketingDBEntities : DbContext
    {
        public TicketingDBEntities()
            : base("name=TicketingDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Counter> Counters { get; set; }
        public virtual DbSet<LatestServed> LatestServeds { get; set; }
        public virtual DbSet<WaitingQueue> WaitingQueues { get; set; }
    }
}
