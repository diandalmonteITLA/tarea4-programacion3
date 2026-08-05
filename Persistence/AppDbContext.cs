using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Domain;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Listing> Listings { get; set; }

    }
}
