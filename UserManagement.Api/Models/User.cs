namespace UserManagement.Api.Models;

public class User
{
    public int Id { get; set; }          // Primary key
    public required string Name { get; set; }     // User full name
    public required string Email { get; set; }    // User email
    public required string Password { get; set; } // Encrypted later
}
