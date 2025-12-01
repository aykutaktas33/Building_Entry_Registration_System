using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.VisitorFeatures.Commands.Validators
{
    public class SavePersonalInfoCommandValidator : AbstractValidator<SavePersonalInfoCommand>
    {
        public SavePersonalInfoCommandValidator()
        {
            RuleFor(x => x.SessionId).NotEmpty().WithMessage("Session ID is required");
        }
    }
}
