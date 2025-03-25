using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoggerNamespace;

namespace SothbeysKillerApi.Controllers;

public record UserCreateRequest(string Name, string Email, string Password);
public record UserSigninRequest(string Email, string Password);
public record UserSigninResponse(Guid id, string Name, string Email);

public class User 
{
    public Guid Id { get; set; }  
    public string Name { get; set; }  
    public string Email { get; set; }  
    private string hashPassword;
    
    public string Password { 
        get => hashPassword;
        set => hashPassword = HashPassword(value);
    }
    private static string HashPassword(string password) {
        // тимчасова версія;
        // поки просто кодуємо у Base64;
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
};


[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase {
    private static List<User> _users = [];
    private readonly Logger _logger;

    public UserController(Logger logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Signup(UserCreateRequest request) {

        int atIndex = request.Email.IndexOf('@');
        string local = request.Email[..atIndex];
        string domain = request.Email[(atIndex + 1)..];
        string topLevelDomain = domain.Split('.')[^1];
        int minimalLengthPassword = 3;


        // code 400
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 3 || request.Name.Length > 255) {
            _logger.LogError("Invalid name in signup request - Code 400");
            return BadRequest();
        }
        else if (string.IsNullOrWhiteSpace(request.Email) || _users.Any(a => a.Email == request.Email)) {
            _logger.LogError($"Invalid or duplicate email '{request.Email}' in signup - Code 400");
            return BadRequest();
        }
        else if (atIndex <= 0) {
            _logger.LogError($"Email '{request.Email}' missing '@' - Code 400");
            return BadRequest();
        }
        else if (domain.Length < 3 || !domain.Contains('.')) {
            _logger.LogError($"Invalid domain in email '{request.Email}' - Code 400");
            return BadRequest();
        }
        else if (topLevelDomain.Length < 2 || topLevelDomain.Length > 6) {
            _logger.LogError($"Invalid top-level domain in email '{request.Email}' - Code 400");
            return BadRequest();
        }
        else if (!IsValidEmailParts(local, domain)) {
            _logger.LogError($"Invalid characters in email '{request.Email}' - Code 400");
            return BadRequest();
        }
        else if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < minimalLengthPassword) {
            _logger.LogError("Invalid password in signup request - Code 400");
            return BadRequest();
        }

        var user = new User() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = request.Password
        };
    
        _users.Add(user);
        // code 204
        return NoContent();
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Signin(UserSigninRequest request) {

        var user = _users.SingleOrDefault(a => a.Email == request.Email);

        if (user is null) {
            // code 404
            _logger.LogError($"User with email '{request.Email}' not found - Code 404");
            return NotFound();
        } else if (!(user.Password.Equals(request.Password))) {
            // code 401
            _logger.LogError($"Invalid password for user '{request.Email}' - Code 401");
            return Unauthorized();
        }

        var response = new UserSigninResponse(user.Id, user.Name, user.Email);

        // code 200
        return Ok(response);
    }

    // Email validation
    private static bool IsValidEmailParts(string local, string domain) {
        // Check if it contains only allowed characters
        bool validLocal = local.All(c => char.IsLetterOrDigit(c) || c is '.' or '-');
        bool validDomain = domain.All(c => char.IsLetterOrDigit(c) || c is '.' or '-');

        // Check if they do not start or end with '.' or '-'
        bool validLocalBoundaries = !(local.StartsWith('.') || local.StartsWith('-') || local.EndsWith('.') || local.EndsWith('-'));
        bool validDomainBoundaries = !(domain.StartsWith('.') || domain.StartsWith('-') || domain.EndsWith('.') || domain.EndsWith('-'));

        return validLocal && validDomain && validLocalBoundaries && validDomainBoundaries;
    }
}