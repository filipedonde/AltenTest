using AltenTest.BookingApi.Application.Commands;
using AltenTest.BookingApi.Data;
using AltenTest.BookingApi.Data.Repository;
using AltenTest.Core.Messages;
using MediatR;

namespace AltenTest.BookingApi.Application.Handlers
{
    public class CancelReservationCommandHandler : CommandHandler, IRequestHandler<CancelReservationCommand, CommandResponse>
    {
        private readonly IReservationRepository _reservationRepository;

        public CancelReservationCommandHandler(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        public async Task<CommandResponse> Handle(CancelReservationCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return new CommandResponse(message.ValidationResult);

            var reservation = await _reservationRepository.GetReservation(message.ReservationNumber);

            if (reservation==null)
            {
                AddError("Reservation Number not found!");
                return new CommandResponse(ValidationResult);
            }

            reservation.IsCancelled = true;

            _reservationRepository.Update(reservation);

            ValidationResult = await SaveData(_reservationRepository.UnitOfWork);

            return ValidationResult.IsValid ? new CommandResponse(message.ReservationNumber) : new CommandResponse(ValidationResult);
        }
    }
}
