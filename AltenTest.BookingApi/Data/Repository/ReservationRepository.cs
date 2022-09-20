using AltenTest.BookingApi.Models;
using AltenTest.Core.Data;
using Microsoft.EntityFrameworkCore;

namespace AltenTest.BookingApi.Data.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly BookingContext _bookingContext;
        public IUnitOfWork UnitOfWork => _bookingContext;

        public ReservationRepository(BookingContext bookingContext)
        {
            _bookingContext = bookingContext;
        }

        public void Add(Reservation reservation)
        {
            _bookingContext.Reservations.Add(reservation);
        }

        public void Update(Reservation reservation)
        {
            _bookingContext.Reservations.Update(reservation);
        }

        public async Task<IEnumerable<Reservation>> GetReservations(DateTime startDate, DateTime endDate)
        {
            return await _bookingContext.Reservations.AsNoTracking()
                .Where(r => !r.IsCancelled &&
                            (
                                (r.StartDate >= startDate && r.StartDate <= endDate) || ((r.EndDate >= startDate && r.EndDate <= endDate))
                            //(r.StartDate <= startDate && r.EndDate >= startDate) || (r.StartDate <= endDate && r.EndDate >= endDate)
                            )
                        )
                .ToListAsync();
        }

        public async Task<bool> IsAvailable(DateTime startDate, DateTime endDate)
        {
            return await IsAvailable(startDate, endDate, null);
        }

        public async Task<bool> IsAvailable(DateTime startDate, DateTime endDate,int? excludeParameter)
        {
            IEnumerable<Reservation> reservations = await GetReservations(startDate, endDate);

            return excludeParameter.HasValue ? !reservations.Any(r => r.ReservationNumber != excludeParameter) : !reservations.Any();
        }

        public async Task<Reservation> GetReservation(int reservationNumber)
        {
            return await _bookingContext.Reservations.FindAsync(reservationNumber);
        }
    }
}
