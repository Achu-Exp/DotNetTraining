using System.ComponentModel.DataAnnotations;

namespace Project21032025.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
