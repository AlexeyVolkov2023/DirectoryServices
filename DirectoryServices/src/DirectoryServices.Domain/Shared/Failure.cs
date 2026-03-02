using System.Collections;

namespace DirectoryServices.Domain.Shared;

public class Failure : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    public Failure(IEnumerable<Error> errors)
    {
        _errors = errors?.ToList() ?? new List<Error>();
    }

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static implicit operator Failure(Error[] errors)
    => new (errors);

    public static implicit operator Failure(Error error)
    => new([error]);
}