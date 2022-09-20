using AltenTest.BookingApi.Application.Commands;
using AltenTest.BookingApi.Data.Repository;
using AltenTest.Core.Messages;
using MediatR;

namespace AltenTest.BookingApi.Application.Handlers
{
    public class ModifyReservationCommandHandler : CommandHandler, IRequestHandler<ModifyReservationCommand, CommandResponse>
    {
        private const string MORE_THAN_3_DAYS_MESSAGE = "The stay can't be longer than 3 days.";
        private const string NOT_AVAILABLE_MESSAGE = "The room is not available in the selected dates.";
        private const string RESERVATION_NOT_FOUND_MESSAGE = "Reservation Number not found!";

        private readonly IReservationRepository _reservationRepository;

        public ModifyReservationCommandHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<CommandResponse> Handle(ModifyReservationCommand message, CancellationToken cancellationToken)
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

            var reservation = await _reservationRepository.GetReservation(message.ReservationNumber);

            if (reservation == null)
            {
                AddError(RESERVATION_NOT_FOUND_MESSAGE);
                return new CommandResponse(ValidationResult);
            }

            reservation.Guest = message.Guest;
            reservation.StartDate = message.StartDate;
            reservation.EndDate = message.EndDate;

            _reservationRepository.Update(reservation);

            ValidationResult = await SaveData(_reservationRepository.UnitOfWork);

            return ValidationResult.IsValid ? new CommandResponse(reservation.ReservationNumber) : new CommandResponse(ValidationResult);
        }
    }
}
