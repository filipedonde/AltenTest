using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltenTest.Core.Messages
{
    public class CommandResponse
    {
        public CommandResponse(int id)
        {
            Id = id;
            IsSucess = true;
        }

        public CommandResponse(ValidationResult validationResult)
        {
            ValidationResult = validationResult;
            IsSucess = false;
        }

        public int? Id { get; set; }
        public bool IsSucess { get; set; }
        public ValidationResult ValidationResult { get; set; }
    }
}
