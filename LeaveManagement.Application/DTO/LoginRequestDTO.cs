﻿namespace LeaveManagement.Application.DTO
{
    public record LoginRequestDTO
    {
       public string Email { get; set; }
       public string Password { get; set; }
    }
}
