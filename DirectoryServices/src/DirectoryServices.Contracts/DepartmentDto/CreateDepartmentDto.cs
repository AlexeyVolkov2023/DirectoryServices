namespace DirectoryServices.Contracts.DepartmentDto;

public record CreateDepartmentDto(
    string DepartmentName,
    string Identifier,
    Guid? ParentId,
    IEnumerable<Guid> LocationIds);