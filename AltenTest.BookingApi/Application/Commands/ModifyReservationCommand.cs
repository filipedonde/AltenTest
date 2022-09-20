using AltenTest.Core.Messages;
using FluentValidation;

namespace AltenTest.BookingApi.Application.Commands
{
    public class ModifyReservationCommand : Command
    {
        public int ReservationNumber { get; set; }
        public string? Guest { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ModifyReservationCommand()
        {

        }

        public ModifyReservationCommand(int reservationNumber, string? guest, DateTime startDate, DateTime endDate)
        {
            ReservationNumber = reservationNumber;
            Guest = guest;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override bool IsValid()
        {
            ValidationResult = new ModifyReservationValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class ModifyReservationValidation : AbstractValidator<ModifyReservationCommand>
    {
        public ModifyReservationValidation()
        {
            RuleFor(c => c.ReservationNumber)
                .NotEmpty();

            RuleFor(b => b.Guest)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(b => b.StartDate)
                .NotEmpty()
                .GreaterThan(DateTime.Today)
                .LessThanOrEqualTo(DateTime.Today.AddDays(30));

            RuleFor(b => b.EndDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(b => b.StartDate)
                .LessThanOrEqualTo(DateTime.Today.AddDays(30));
        }
    }
}
