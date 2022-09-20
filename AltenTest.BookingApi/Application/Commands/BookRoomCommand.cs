using AltenTest.Core.Messages;
using FluentValidation;

namespace AltenTest.BookingApi.Application.Commands
{
    public class BookRoomCommand : Command
    {
        public string? Guest { get; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public BookRoomCommand()
        {
        }

        public BookRoomCommand(string guest, DateTime startDate, DateTime endDate)
        {
            Guest = guest;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override bool IsValid()
        {
            ValidationResult = new BookRoomValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }

    public class BookRoomValidation : AbstractValidator<BookRoomCommand>
    {
        public BookRoomValidation()
        {
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
