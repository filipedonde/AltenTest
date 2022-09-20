using AltenTest.BookingApi.Application.Commands;
using AltenTest.BookingApi.Models.ApiModels;
using AltenTest.BookingApi.Services;
using AltenTest.Core.Mediator;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AltenTest.BookingApi.Controllers
{
    public class BookingController : MainController
    {
        private readonly IMediatorHandler _mediator;
        private readonly IBookingServices _bookingServices;

        public BookingController(IMediatorHandler mediator, IBookingServices bookingServices)
        {
            _mediator = mediator;
            _bookingServices = bookingServices;
        }

        [HttpGet("booking/{reservationNumber}")]
        public async Task<IActionResult> GetReservationInfo(int reservationNumber)
        {
            return CustomResponse(await _bookingServices.GetReservation(reservationNumber));
        }

        [HttpGet("booking/check-availability")]
        public async Task<IActionResult> CheckAvailability(DateTime startDate, DateTime endDate)
        {
            if (!IsValidDatesRequest(startDate, endDate))
            {
                return CustomResponse();
            }

            return CustomResponse(await _bookingServices.CheckAvailability(startDate, endDate));
        }

        [HttpDelete("booking/cancel-reservation/{reservationNumber}")]
        public async Task<IActionResult> CancelReservation(int reservationNumber)
        {
            return CustomResponse(await _mediator.SendCommand(new CancelReservationCommand(reservationNumber)));
        }

        [HttpPut("booking/modify-reservation/{reservationNumber}")]
        public async Task<IActionResult> ModifyReservation(int reservationNumber, BookRoomPostData bookingData)
        {
            return CustomResponse(await _mediator.SendCommand(new ModifyReservationCommand(reservationNumber, bookingData.Guest, bookingData.StartDate.Date, bookingData.EndDate.Date)));
        }


        [HttpPost("booking/book-room")]
        public async Task<IActionResult> BookRoom(BookRoomPostData bookingData)
        {
            return CustomResponse(await _mediator.SendCommand(new BookRoomCommand(bookingData.Guest, bookingData.StartDate.Date, bookingData.EndDate.Date)));
        }

        private bool IsValidDatesRequest(DateTime startDate, DateTime endDate)
        {
            bool isValid = true;

            if (startDate > endDate)
            {
                AddProcessingError("Start Date cannot be greater than End Date.");
            }

            if (startDate <= DateTime.Today)
            {
                AddProcessingError("The room can be reserved at least the next day of booking.");
            }

            if ((endDate - DateTime.Today).TotalDays > 30)
            {
                AddProcessingError("The room can't be reserved with more than 30 days in advance.");
            }

            return isValid;
        }
    }
}
