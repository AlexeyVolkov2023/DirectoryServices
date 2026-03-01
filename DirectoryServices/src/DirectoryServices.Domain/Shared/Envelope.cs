using System.Text.Json.Serialization;

namespace DirectoryServices.Domain.Shared;

public record Envelope
{
    public object? Result { get; }

    public Error? Error { get; }

    public bool IsError => Error != null;

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    private Envelope(object? result, Error? error)
    {
        Result = result;
        Error = error;
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Fail(Error error) =>
        new(null, error);
}

public record Envelope<T>
{
    public T? Result { get; }

    public Failure? Errors { get; }

    public bool IsError => Errors != null || (Errors != null && Errors.Any());

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    private Envelope(T? result, Failure? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope<T> Ok(T? result = default) =>
        new(result, null);

    public static Envelope<T> Error(Failure errors) =>
        new(default, errors);
}