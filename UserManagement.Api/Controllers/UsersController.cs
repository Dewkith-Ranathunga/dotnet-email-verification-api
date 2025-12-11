using Microsoft.AspNetCore.Mvc; // for controller functionalities like routing, HTTP methods etc
using Microsoft.EntityFrameworkCore; // for database operations using Entity Framework Core like querying, saving changes etc
using UserManagement.Api.Data; // for accessing the AppDbContext which represents the database session like tables and relationships
using UserManagement.Api.Models; // for accessing the User model which represents the user entity in the database like properties and validation
using MailKit.Net.Smtp; // for sending emails using SMTP protocol like connecting to server, authenticating, sending messages etc
using MimeKit; // for creating and manipulating email messages like setting sender, recipient, subject, body etc

namespace UserManagement.Api.Controllers; // namespace declaration for organizing code and avoiding name conflicts

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UsersController(AppDbContext context, IConfiguration config)
    {
        _context = context; // initialize database context for CRUD operations
        _config = config; // initialize configuration for accessing app settings like email credentials
    }

    // -------------------------------------------------------------
    // REGISTER + SEND EMAIL TOKEN
    // -------------------------------------------------------------
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        // generate verification token
        user.VerificationToken = Guid.NewGuid().ToString(); // Guid is a globally unique identifier used to create unique tokens
        user.VerificationTokenExpiry = DateTime.Now.AddHours(1); // token valid for 1 hour

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // send email
        await SendVerificationEmail(user.Email, user.VerificationToken!);

        return Ok("Registered successfully. Please check your email to verify.");
    }

    // FUNCTION: Send email
    private async Task SendVerificationEmail(string email, string token)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("User System", _config["EmailSettings:Email"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Verify your email";

        string verifyUrl = $"http://localhost:5077/api/users/verify?token={token}";

        message.Body = new TextPart("plain")
        {
            Text = $"Click here to verify your email: {verifyUrl}"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_config["EmailSettings:Host"], int.Parse(_config["EmailSettings:Port"]), false);
        await client.AuthenticateAsync(_config["EmailSettings:Email"], _config["EmailSettings:Password"]);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    // -------------------------------------------------------------
    // VERIFY EMAIL
    // -------------------------------------------------------------
    [HttpGet("verify")]
    public async Task<IActionResult> VerifyEmail(string token)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

        if (user == null || user.VerificationTokenExpiry < DateTime.Now)
            return BadRequest("Invalid or expired token");

        user.IsEmailVerified = true;
        user.VerificationToken = null;
        user.VerificationTokenExpiry = null;

        await _context.SaveChangesAsync();

        return Ok("Email verified successfully!");
    }

    // -------------------------------------------------------------
    // LOGIN (email must be verified)
    // -------------------------------------------------------------
    [HttpPost("login")]
    public async Task<IActionResult> Login(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

        if (user == null)
            return BadRequest("Invalid email or password");

        if (!user.IsEmailVerified)
            return BadRequest("Please verify your email first.");

        return Ok("Login Successful!");
    }


    // -------------------------------------------------------------
    // NORMAL CRUD
    // -------------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        return Ok(await _context.Users.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, User updatedUser)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Name = updatedUser.Name;
        user.Email = updatedUser.Email;
        user.Password = updatedUser.Password;

        await _context.SaveChangesAsync();
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok("Deleted");
    }
}
