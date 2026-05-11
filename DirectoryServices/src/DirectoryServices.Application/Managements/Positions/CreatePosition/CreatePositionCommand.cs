using DirectoryServices.Application.Abstractions;
using DirectoryServices.Contracts.PositionDto;

namespace DirectoryServices.Application.Managements.Positions.CreatePosition;

public record CreatePositionCommand(CreatePositionDto CreatePositionDto) : ICommand;