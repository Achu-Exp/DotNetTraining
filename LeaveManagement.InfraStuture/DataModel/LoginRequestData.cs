using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Infrastructure.DataModel
{
    public record LoginRequestData
    (
        string Email,
        string Password
    );
}
