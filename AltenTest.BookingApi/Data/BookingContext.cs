using AltenTest.BookingApi.Models;
using AltenTest.Core.Data;
using AltenTest.Core.Mediator;
using AltenTest.Core.Messages;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;

namespace AltenTest.BookingApi.Data
{
    public class BookingContext : DbContext, IUnitOfWork
    {
        public BookingContext(DbContextOptions options) : base(options)
        {

            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<ValidationResult>();
            modelBuilder.Ignore<Event>();

            foreach (var property in modelBuilder.Model.GetEntityTypes().SelectMany(
                e => e.GetProperties().Where(p => p.ClrType == typeof(string))))
                property.SetColumnType("varchar(100)");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookingContext).Assembly);
        }

        public async Task<bool> Commit()
        {
            var sucesso = await base.SaveChangesAsync() > 0;

            return sucesso;
        }
    }
}
