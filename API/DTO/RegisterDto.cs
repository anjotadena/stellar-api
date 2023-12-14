using System.ComponentModel.DataAnnotations;

namespace API.DTO;
public class RegisterDto
{
    [Required]
    public string DisplayName { get; set; }
    
    [Required]
    public string Email { get; set; }

    [Required]
    [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "Your password must be at least 12 characters long and include a mix of uppercase and lowercase letters, numbers, and special characters.")]
    public string Password { get; set; }
}