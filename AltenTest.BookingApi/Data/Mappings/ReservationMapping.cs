using AltenTest.BookingApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AltenTest.BookingApi.Data.Mappings
{
    public class ReservationMapping : IEntityTypeConfiguration<Reservation>
    {
        public void Configure(EntityTypeBuilder<Reservation> builder)
        {
            builder.HasKey(b => b.ReservationNumber);
            builder.Property(b=>b.ReservationNumber).UseIdentityColumn(2000);

            builder.Property(b => b.Guest)
                .IsRequired()
                .HasColumnType("varchar(100)");
            
            builder.Property(b => b.StartDate)
                .IsRequired();

            builder.Property(b => b.EndDate)
                .IsRequired();

            builder.ToTable("Reservations");
        }
    }
}
