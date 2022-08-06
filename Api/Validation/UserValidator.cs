using Entities.DataContract;
using FluentValidation;

namespace Api.Validation;

// ReSharper disable once ClassNeverInstantiated.Global - Used internally by FluentValidation
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(user => user.name)
            .NotNull();
    }
}