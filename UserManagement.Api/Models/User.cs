namespace UserManagement.Api.Models;

public class User
{
    public int Id { get; set; }

    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }

    // Email verification fields
    public bool IsEmailVerified { get; set; } = false;
    public string? VerificationToken { get; set; }
    public DateTime? VerificationTokenExpiry { get; set; }
}
