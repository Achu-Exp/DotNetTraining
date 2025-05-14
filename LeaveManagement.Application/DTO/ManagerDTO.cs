namespace LeaveManagement.Application.DTO
{
    public record ManagerDTO
    {
       public int Id { get; set; }
       public UsersDTO User { get; set; }
    }
}
