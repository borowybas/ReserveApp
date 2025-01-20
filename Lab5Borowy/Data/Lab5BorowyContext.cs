using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab5Borowy.Models;

namespace Lab5Borowy.Data
{
    public class Lab5BorowyContext : DbContext
    {
        public Lab5BorowyContext (DbContextOptions<Lab5BorowyContext> options)
            : base(options)
        {
        }

        public DbSet<Lab5Borowy.Models.Book> Book { get; set; } = default!;

        public DbSet<User> Users { get; set; }
    }
}
