using AltenTest.Core.Messages;
using FluentValidation;

namespace AltenTest.BookingApi.Application.Commands
{
    public class CancelReservationCommand : Command
    {
        public int ReservationNumber { get; set; }


        public CancelReservationCommand(int reservationNumber)
        {
            ReservationNumber = reservationNumber;
        }

        public CancelReservationCommand() { }

        public override bool IsValid()
        {
            ValidationResult = new CancelReservationValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class CancelReservationValidation : AbstractValidator<CancelReservationCommand>
    {
        public CancelReservationValidation()
        {
            RuleFor(b => b.ReservationNumber)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
