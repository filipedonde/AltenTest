using AltenTest.BookingApi.Models;
using AltenTest.Core.Data;

namespace AltenTest.BookingApi.Data.Repository
{
    public interface IReservationRepository
    {
        IUnitOfWork UnitOfWork { get; }
        void Add(Reservation reservation);
        void Update(Reservation reservation);
        Task<Reservation> GetReservation(int reservationNumber);
        Task<IEnumerable<Reservation>> GetReservations(DateTime startDate, DateTime endDate);
        Task<bool> IsAvailable(DateTime startDate, DateTime endDate);
    }
}