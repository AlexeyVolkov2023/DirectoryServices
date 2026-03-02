namespace DirectoryServices.Domain.Shared;

public static class GeneralErrors
{
    public static Error ValueIsInvalid(string? name = null)
    {
        string label = name ?? "value";
        return Error.Validation("value.is.invalid", $"{label} is invalid");
    }

    public static Error NotFound(Guid? id = null, string? name = null)
    {
        string forId = id == null ? string.Empty : $" for Id '{id}'";
        return Error.NotFound("record.not.found", $"{name ?? "record"} not found {forId}");
    }

    public static Error ValueIsRequired(string? name = null)
    {
        string label = name == null ? string.Empty : name;
        return Error.Validation("value.is.required", $"Not specified: {label}");
    }

    public static Error AlreadyExist()
    {
        return Error.Conflict("record.already.exists", "Record already exists");
    }

    public static Error Failure(string? message = null)
    {
        return Error.Failure("server.failure", message ?? "Server error");
    }
}