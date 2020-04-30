using FluentValidation;
using SportBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportBook.Helpers
{
    public class EventDateValidator : AbstractValidator<Event>
    {
        public EventDateValidator()
        {
            RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage("End time must be greater that start time");
        }
    }
}
