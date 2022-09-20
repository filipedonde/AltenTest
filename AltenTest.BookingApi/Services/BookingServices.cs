using AltenTest.BookingApi.Data;
using AltenTest.BookingApi.Data.Repository;
using AltenTest.BookingApi.Models;
using AltenTest.BookingApi.Models.ApiModels;

namespace AltenTest.BookingApi.Services
{

    public class BookingServices : IBookingServices
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly BookingContext _bookingContext;

        public BookingServices(IReservationRepository reservationRepository, BookingContext bookingContext)
        {
            _reservationRepository = reservationRepository;
            _bookingContext = bookingContext;
        }

        public async Task<Reservation> GetReservation(int reservationNumber)
        {
            return await _bookingContext.Reservations.FindAsync(reservationNumber);
        }

        public async Task<AvailabilityResponse> CheckAvailability(DateTime startDate, DateTime endDate)
        {
            var reservationsInDate = await _reservationRepository.GetReservations(startDate, endDate);

            var response = new AvailabilityResponse();

            DateTime currentDayChecking = startDate;

            while (currentDayChecking <= endDate)
            {
                if (!reservationsInDate.Any(r => r.StartDate <= currentDayChecking && r.EndDate >= currentDayChecking))
                {
                    response.AvailableDays.Add(currentDayChecking);
                }

                currentDayChecking = currentDayChecking.AddDays(1);
            }

            return response;
        }
    }
}
