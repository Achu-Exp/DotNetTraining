using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeaveManagement.Infrastructure.DataModel;

namespace LeaveManagement.Infrastructure.Repositories.Interfaces
{
    public interface IAuthRepository
    {
        Task<(User?, string)> LoginUser(LoginRequestData loginRequest);
    }
}
