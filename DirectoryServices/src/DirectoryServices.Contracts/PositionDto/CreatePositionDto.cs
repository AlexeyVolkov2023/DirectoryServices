namespace DirectoryServices.Contracts.PositionDto;

public record CreatePositionDto(
    string PositionName,
    string? Description,
    IEnumerable<Guid> DepartmentIds);