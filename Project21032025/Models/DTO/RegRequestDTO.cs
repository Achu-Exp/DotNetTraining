namespace Project21032025.Models.DTO
{
    public class RegRequestDTO
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  // "Employee", "Manager", "Admin"
    }
}
