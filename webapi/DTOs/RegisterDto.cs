using System.ComponentModel.DataAnnotations;

namespace webapi.DTOs;

public class RegisterDto
{
    [Required]
    public string UserName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [MinLength(6)]
    public string Password { get; set; }
}