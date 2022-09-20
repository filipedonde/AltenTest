using AltenTest.BookingApi.Models;
using AltenTest.BookingApi.Models.ApiModels;

namespace AltenTest.BookingApi.Services
{
    public interface IBookingServices
    {
        Task<Reservation> GetReservation(int reservationNumber);
        Task<AvailabilityResponse> CheckAvailability(DateTime startDate, DateTime endDate);
    }
}
