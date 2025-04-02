using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeaveManagement.Application.DTO
{
    public record ManagerDTO
    {
       public int Id { get; set; }

        public UserDTO User { get; set; }

       //public int UserId { get; set; }

       //public int DepartmentId { get; set; }
       //public string Name { get; set; }
       //public string Email { get; set; }
       //public string Address { get; set; }
    }
}
