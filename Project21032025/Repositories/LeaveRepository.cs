using Microsoft.EntityFrameworkCore;
using Project21032025.Data;
using Project21032025.Models;
using Project21032025.Repositories.Interfaces;

namespace Project21032025.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddLeaveRequest(LeaveRequest request)
        {
            await _db.LeaveRequests.AddAsync(request);
            await _db.SaveChangesAsync();
        }

        public Task DeleteLeaveRequest(int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests()
        {
            return await _db.LeaveRequests.ToListAsync();
        }

        public Task<LeaveRequest> GetLeaveRequest(int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLeaveRequest(LeaveRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
