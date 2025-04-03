namespace LeaveManagement.Infrastructure.DataModel
{
    public record LeaveRequestData
    (

        int Id,
        DateOnly StartDate,
        DateOnly EndDate,
        string Reason,
        string Status,
        int EmployeeId,
        int? ApproverId

    );
}
