using Microsoft.EntityFrameworkCore;
using LeaveMangementSystem.Data;
using LeaveMangementSystem.Models;
using LeaveMangementSystem.Repositories.Interfaces;
using LeaveMangementSystem.Models.DTO;

namespace LeaveMangementSystem.Repositories
{
    public class LeaveRepository : ILeaveRepository
    {
        private readonly ApplicationDbContext _db;

        public LeaveRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddLeaveRequest(LeaveRequest leaveRequest)
        {
            leaveRequest.Status = leaveRequest.Status ?? "Pending"; 
            await _db.LeaveRequests.AddAsync(leaveRequest);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteLeaveRequest(int userId)
        {
            var leaveRequest = await _db.LeaveRequests.FindAsync(userId);
            if (leaveRequest != null)
            {
                _db.LeaveRequests.Remove(leaveRequest);
                _db.SaveChanges();
            }
        }

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequests()
        {
            return await _db.LeaveRequests.ToListAsync();
        }

        public async Task<LeaveRequest> GetLeaveRequestById(int userId)
        {
            return await _db.LeaveRequests.FindAsync(userId);
        }

        public async Task UpdateLeaveRequest(LeaveRequest request)
        {
            _db.LeaveRequests.Update(request);
            await _db.SaveChangesAsync();
        }
    }
}
