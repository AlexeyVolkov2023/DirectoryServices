using System.Text.Json;
using DirectoryServices.Domain.Shared;
using FluentValidation.Results;

namespace DirectoryServices.Application.Extensions;

public static class ValidationExtension
{
    public static Error ToError(this ValidationResult validationResult)
    {
        List<ValidationFailure> validationErrors = validationResult.Errors;

        IEnumerable<IReadOnlyList<ErrorMessage>> errors =
            from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = JsonSerializer.Deserialize<Error>(errorMessage)
            select error.Messages;

        return Error.Validation(errors.SelectMany(e => e));
    }

    public static Errors ToErrors(this ValidationResult validationResult) =>
        validationResult.Errors.Select(e => Error.Validation(e.ErrorCode, e.ErrorMessage, e.PropertyName)).ToArray();
}