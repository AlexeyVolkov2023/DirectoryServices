using System.Text.Json.Serialization;

namespace DirectoryServices.Domain.Shared;

public record Error
{
    public IReadOnlyList<ErrorMessage> Messages { get; } = [];

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ErrorType Type { get; }


    private Error(IEnumerable<ErrorMessage> messages, ErrorType type)
    {
        Messages = messages.ToList();
        Type = type;
    }

    public static Error Validation(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.VALIDATION);

    public static Error NotFound(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.NOT_FOUND);

    public static Error Failure(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.FAILURE);

    public static Error Conflict(string code, string message, string? invalidField = null) =>
        new([new ErrorMessage(code, message, invalidField)], ErrorType.CONFLICT);

    public static Error Validation(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.VALIDATION);

    public static Error NotFound(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.NOT_FOUND);

    public static Error Failure(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.FAILURE);

    public static Error Conflict(params IEnumerable<ErrorMessage> messages) =>
        new(messages, ErrorType.CONFLICT);

    /*public Failure ToErrors => this;*/
}

public enum ErrorType
{
    VALIDATION,
    NOT_FOUND,
    FAILURE,
    CONFLICT,
}

public record ErrorMessage(string code, string message, string? invalidField = null);