using System.Text.Json.Serialization;

namespace DirectoryServices.Domain.Shared;

public record Envelope
{
    public object? Result { get; }

    public Errors? Errors { get; }

    public bool IsError => Errors != null;

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    private Envelope(object? result, Errors? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.UtcNow;
    }

    public static Envelope Ok(object? result = null) =>
        new(result, null);

    public static Envelope Fail(Errors error) =>
        new(null, error);
}

public record Envelope<T>
{
    public T? Result { get; }

    public Errors? Errors { get; }

    public bool IsError => Errors != null || (Errors != null && Errors.Any());

    public DateTime TimeGenerated { get; }

    [JsonConstructor]
    private Envelope(T? result, Errors? errors)
    {
        Result = result;
        Errors = errors;
        TimeGenerated = DateTime.Now;
    }

    public static Envelope<T> Ok(T? result = default) =>
        new(result, null);

    public static Envelope<T> Error(Errors errors) =>
        new(default, errors);
}