namespace DirectoryServices.Domain.Department.Id;

public record DepartmentId
{
    private DepartmentId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static DepartmentId NewDepartmentId() => new(Guid.NewGuid());

    public static DepartmentId Empty() => new(Guid.Empty);

    public static DepartmentId Create(Guid id) => new(id);

    public static implicit operator DepartmentId(Guid id) => new DepartmentId(id);

    public static implicit operator Guid(DepartmentId departmentId)
    {
        ArgumentNullException.ThrowIfNull(departmentId);

        return departmentId.Value;
    }
}