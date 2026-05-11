using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Domain.DepartmentManagement.ValueObjects;

public record Depth
{
    private Depth(short value)
    {
        Value = value;
    }

    public short Value { get; }

    public static Result<Depth, Error> Create(short value)
    {
        if (value < 0)
            return GeneralErrors.ValueIsInvalid("depth");

        return new Depth(value);
    }

    public Result<Depth, Error> Increment()
    {
        if (Value >= short.MaxValue)
        {
            return GeneralErrors.ValueIsInvalid("depth");
        }

        var newValue = (short)(Value + 1);
        return new Depth(newValue);
    }

    public Result<Depth, Error> Decrement()
    {
        if (Value <= short.MinValue)
        {
            return GeneralErrors.ValueIsInvalid("depth");
        }

        var newValue = (short)(Value - 1);
        return new Depth(newValue);
    }

    public static Depth RootDepth()
    {
        return new Depth(0);
    }
}