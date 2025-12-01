using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.EntranceFeatures.Queries.Validators
{
    public class ValidateEntranceFeaturesQueryValidator : AbstractValidator<ValidateEntranceFeaturesQuery>
    {
        public ValidateEntranceFeaturesQueryValidator()
        {
            RuleFor(x => x.EntranceId)
                .NotEmpty().WithMessage("Entrance ID is required")
                .MaximumLength(50).WithMessage("Entrance ID cannot be longer than 50 characters");
        }
    }
}
