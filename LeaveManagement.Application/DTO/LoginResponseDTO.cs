﻿namespace LeaveManagement.Application.DTO
{
    public record LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Token {  get; set; }
    }
}
