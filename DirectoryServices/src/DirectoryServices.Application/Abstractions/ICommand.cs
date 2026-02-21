using CSharpFunctionalExtensions;

namespace DirectoryServices.Application.Abstractions;

public interface ICommand;

public interface ICommandHandler<TResponse, in TCommand>
    where TCommand : ICommand
{
    Task<Result<TResponse>> Handle(TCommand command, CancellationToken cancellationToken);
}

/*public interface ICommandHandler<in TCommand>
    where TCommand : ICommand
{
    Task<UnitResult<Failure>> Handle(TCommand command, CancellationToken cancellationToken);
}*/