namespace LeaveManagement.Infrastructure.DataModel
{
    public record EmployeeData
    (
        int Id,
        UserData User,  
        int? ManagerId
    );
}
