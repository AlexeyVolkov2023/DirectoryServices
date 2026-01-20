using CSharpFunctionalExtensions;

namespace DirectoryServices.Domain.Department.ValueObjects;

public record Depth
{
    private Depth(short value)
    {
        Value = value;
    }

    public short Value { get; }

    public Result<Depth> Create(short value)
    {
        if (value <= 0)
            return Result.Failure<Depth>("Depth must be a positive number.");

        return Result.Success<Depth>(new Depth(value));
    }
}