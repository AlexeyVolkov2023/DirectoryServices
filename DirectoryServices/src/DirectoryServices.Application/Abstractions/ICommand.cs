using CSharpFunctionalExtensions;
using DirectoryServices.Domain.Shared;

namespace DirectoryServices.Application.Abstractions;

public interface ICommand;

public interface ICommandHandler<TResponse, in TCommand>
    where TCommand : ICommand
{
    Task<Result<Guid, Error>> Handle(TCommand command, CancellationToken cancellationToken);
}

/*public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Failure>> Handle(TCommand command, CancellationToken cancellationToken);
}*/