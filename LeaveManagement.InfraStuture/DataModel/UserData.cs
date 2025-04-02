namespace LeaveManagement.Infrastructure.DataModel
{
    public record UserData
    (
        int Id,
        string Name,
        string Email,
        string Address,
        int DepartmentId
    );
}
