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

        public async Task<List<LeaveRequestData>> GetAllAsync(DateOnly? startDate = null, DateOnly? endDate = null,
            LeaveStatus? status = LeaveStatus.Pending, int? employeeId = null, int? approverId = null,
            string? sortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 100)
        {
            var leaveRequests = _context.LeaveRequests.Select(e => new LeaveRequestData
                (
                    e.Id,
                    e.StartDate,
                    e.EndDate,
                    e.Reason,
                    e.Status,
                    e.EmployeeId,
                    e.ApproverId
                ))
                .AsQueryable();

            // Filtering
            if (startDate != null)
            {
                leaveRequests = leaveRequests.Where(x => x.StartDate == startDate);
            }

            if (endDate != null)
            {
                leaveRequests = leaveRequests.Where(x => x.EndDate == endDate);
            }

            if (status != null)
            {
                leaveRequests = leaveRequests.Where(x => x.Status == status.Value);
            }

            if (employeeId != null)
            {
                leaveRequests = leaveRequests.Where(x => x.EmployeeId == employeeId);
            }

            if (approverId != null)
            {
                leaveRequests = leaveRequests.Where(x => x.ApproverId == approverId);
            }

            // Sorting
            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("StartDate", StringComparison.OrdinalIgnoreCase))
                {
                    leaveRequests = isAscending ? leaveRequests.OrderBy(x => x.StartDate) : leaveRequests.OrderByDescending(x => x.StartDate);
                }
                else if (sortBy.Equals("EndDate", StringComparison.OrdinalIgnoreCase))
                {
                    leaveRequests = isAscending ? leaveRequests.OrderBy(x => x.EndDate) : leaveRequests.OrderByDescending(x => x.EndDate);
                }
                else if (sortBy.Equals("Status", StringComparison.OrdinalIgnoreCase))
                {
                    leaveRequests = isAscending ? leaveRequests.OrderBy(x => x.Status) : leaveRequests.OrderByDescending(x => x.Status);
                }
            }

            //Pagination
            var skipResults = (pageNumber - 1) * pageSize;

            return await leaveRequests.Skip(skipResults).Take(pageSize).ToListAsync();
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
                        e.Status,
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

        public async Task<List<LeaveRequestData>> GetLeaveRequestByApprover(int id)
        {
            return await _context.LeaveRequests
           .Where(e => e.ApproverId == id)
           .Select(e => new LeaveRequestData
           (
               e.Id,
               e.StartDate,
               e.EndDate,
               e.Reason,
               e.Status,
               e.EmployeeId,
               e.ApproverId
           )).ToListAsync();
        }
    }
}
