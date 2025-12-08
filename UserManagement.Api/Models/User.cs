namespace UserManagement.Api.Models;

public class User
{
    public int Id { get; set; }          // Primary key
    public string Name { get; set; }     // User full name
    public string Email { get; set; }    // User email
    public string Password { get; set; } // Encrypted later
}
