using AltenTest.BookingApi.Application.Commands;
using AltenTest.BookingApi.Data;
using AltenTest.BookingApi.Data.Repository;
using AltenTest.Core.Messages;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace AltenTest.BookingApi.Application.Handlers
{
    public class BookRoomCommandHandler : CommandHandler, IRequestHandler<BookRoomCommand, CommandResponse>
    {
        private const string MORE_THAN_3_DAYS_MESSAGE = "The stay can't be longer than 3 days.";
        private const string NOT_AVAILABLE_MESSAGE = "The room is not available in the selected dates.";
        //private readonly BookingContext _bookingContext;
        private readonly IReservationRepository _reservationRepository;


        public BookRoomCommandHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<CommandResponse> Handle(BookRoomCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return new CommandResponse(message.ValidationResult);

            if ((message.EndDate - message.StartDate).TotalDays > 2)
            {
                AddError(MORE_THAN_3_DAYS_MESSAGE);
                return new CommandResponse(ValidationResult);
            }

            if (!await _reservationRepository.IsAvailable(message.StartDate, message.EndDate))
            {
                AddError(NOT_AVAILABLE_MESSAGE);
                return new CommandResponse(ValidationResult);
            }

            Models.Reservation newReservation = new Models.Reservation { Guest = message.Guest, EndDate = message.EndDate, StartDate = message.StartDate };
            _reservationRepository.Add(newReservation);

            ValidationResult= await SaveData(_reservationRepository.UnitOfWork);

            return ValidationResult.IsValid ? new CommandResponse(newReservation.ReservationNumber) : new CommandResponse(ValidationResult);
        }
    }
}
