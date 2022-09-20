﻿using System;
using FluentValidation.Results;
using MediatR;

namespace AltenTest.Core.Messages
{
    public abstract class Command : Message, IRequest<CommandResponse>
    {
        public DateTime Timestamp { get; private set; }
        public ValidationResult ValidationResult { get; set; }

        protected Command()
        {
            Timestamp = DateTime.Now;
        }

        public abstract bool IsValid();
    }
}