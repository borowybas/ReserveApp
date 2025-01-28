using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Lab5Borowy.Models;
//using ReserveApp.Models;

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

        public DbSet<SportClass> SportClasses { get; set; } = default!;

        public DbSet<Reservation> Reservations { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Ignorowanie  delegata ValidateReservation
            modelBuilder.Entity<SportClass>().Ignore(e => e.ValidateReservation);

        }
    }
}
