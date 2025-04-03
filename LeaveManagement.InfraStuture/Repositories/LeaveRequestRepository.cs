using LeaveManagement.Infrastructure.DataModel;
using LeaveManagement.Infrastructure.Repositories.Interfaces;

namespace LeaveManagement.Infrastructure.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly ApplicationDbContext _context;

        public LeaveRequestRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(LeaveRequest leaveRequest)
        {
            await _context.LeaveRequests.AddAsync(leaveRequest);
        }
        public async Task<LeaveRequest?> FindAsync(int id)
        {
            return await _context.LeaveRequests.FindAsync(id);
        }

        public async Task<List<LeaveRequestData>> GetAllAsync()
        {
            return await _context.LeaveRequests.Select(e => new LeaveRequestData
                (
                    e.Id,
                    e.StartDate,
                    e.EndDate,
                    e.Reason,   
                    e.Status.ToString(),   
                    e.EmployeeId,
                    e.ApproverId
                )
                ).ToListAsync();
        }

        public async Task<LeaveRequestData?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests
               .Where(e => e.Id == id)
               .Select(e => new LeaveRequestData
                  (
                        e.Id,
                        e.StartDate,
                        e.EndDate,
                        e.Reason,
                        e.Status.ToString(),
                        e.EmployeeId,
                        e.ApproverId

                   )
               ).FirstOrDefaultAsync();
        }

        public async Task<LeaveRequest> UpdateAsync(LeaveRequest leaveRequest)
        {
            _context.LeaveRequests.Update(leaveRequest);
            return await Task.FromResult(leaveRequest);
        }
        public async Task<bool> DeleteAsync(LeaveRequest? leaveRequest)
        {
            var entity = _context.LeaveRequests.Remove(leaveRequest);
            return await Task.FromResult(true);
        }
    }
}
