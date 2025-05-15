using LeaveManagement.Domain.Entities;

namespace LeaveManagementSystem.Application.Services
{
    // Notifier class (define and raise event)
    public class LeaveRequestNotifier
    {
        // Event
        public event EventHandler<LeaveRequestEventArgs>? LeaveRequestCreated;
        public event EventHandler<LeaveStatusChangedEventArgs>? LeaveStatusChanged;
        public void RaiseLeaveRequestCreated(int employeeId, int approverId)
        {
            LeaveRequestCreated?.Invoke(this, new LeaveRequestEventArgs(employeeId, approverId));
        }

        public void RaiseLeaveStatusChanged(LeaveRequest leave, User employeeUser)
        {
            LeaveStatusChanged?.Invoke(this, new LeaveStatusChangedEventArgs
            {
                LeaveRequestId = leave.Id,
                EmployeeId = leave.EmployeeId,
                ApproverId = leave.ApproverId,
                StartDate = leave.StartDate.ToDateTime(TimeOnly.MinValue),
                EndDate = leave.EndDate.ToDateTime(TimeOnly.MinValue),
                Status = leave.Status
            });
        }
    }

    // Custom event class
    // To add leave requests
    public class LeaveRequestEventArgs : EventArgs
    {
        public int EmployeeId { get; set; }
        public int ApproveId { get; set; }
        public LeaveRequestEventArgs(int employeeId, int approveId)
        {
            EmployeeId = employeeId;
            ApproveId = approveId;
        }
    }

    // Define EventArgs class (the data passed to subscribers)
    public class LeaveStatusChangedEventArgs : EventArgs
    {
        public int LeaveRequestId { get; set; }
        public int EmployeeId { get; set; }
        public int? ApproverId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public LeaveStatus Status { get; set; }
    }
}
