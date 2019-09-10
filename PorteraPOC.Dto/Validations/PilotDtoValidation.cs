using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace PorteraPOC.Dto.Validations
{
    public class PilotDtoValidation : AbstractValidator<PilotDto>
    {
        public PilotDtoValidation()
        {
            RuleFor(pilot => pilot.Id).NotEmpty().WithMessage("Parameter can not be empty.Please Check your query string.");
            RuleFor(pilot => pilot.Id).Must(pilot => pilot.Length == 11).WithMessage("Sent parameter value must be 11 charachters!");
        }
    }
}
