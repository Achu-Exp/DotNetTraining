namespace LeaveManagement.Application.DTO
{
    public record LoginResponseDTO
    {
        public EmployeeDTO User { get; set; }
        public string Token {  get; set; }
    }
}
