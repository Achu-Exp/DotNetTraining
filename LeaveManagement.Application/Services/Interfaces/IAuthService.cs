using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<DTO.LoginResponseDTO> LoginUser(DTO.LoginRequestDTO loginRequest);
    }
}
