using AltenTest.Core.Data;
using FluentValidation.Results;

namespace AltenTest.Core.Messages
{
    public abstract class CommandHandler
    {
        protected ValidationResult ValidationResult;

        protected CommandHandler()
        {
            ValidationResult = new ValidationResult();
        }

        protected void AddError(string message)
        {
            ValidationResult.Errors.Add(new ValidationFailure(string.Empty, message));
        }

        protected async Task<ValidationResult> SaveData(IUnitOfWork uow)
        {
            if (!await uow.Commit()) AddError("There is an error while saving Data!");

            return ValidationResult;
        }
    }
}