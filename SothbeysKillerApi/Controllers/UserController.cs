using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LoggerNamespace;
using System.Text.RegularExpressions;

namespace SothbeysKillerApi.Controllers;
public record UserCreateRequest(string Name, string Email, string Password);
public record UserSigninRequest(string Email, string Password);
public record UserSigninResponse(Guid id, string Name, string Email);

public class User {
    public Guid Id { get; set; }  
    public string Name { get; set; }  
    public string Email { get; set; }  
    public string Password { get; set;}
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

        // code 400
        if (string.IsNullOrWhiteSpace(request.Email))  {
            _logger.LogError("Empty email in signup request - Code 400");
            return BadRequest();
        }

        // індекс символу перед @
        int atIndex = request.Email.IndexOf('@');

        if (atIndex <= 0 || atIndex == request.Email.Length - 1) {
            _logger.LogError($"Email '{request.Email}' missing '@' or domain - Code 400");
            return BadRequest();
        }

        // від початку до індексу
        string local = request.Email[..atIndex];
        // все що після @
        string domain = request.Email[(atIndex + 1)..];

        if (!domain.Contains('.')) {
            _logger.LogError($"Invalid domain in email '{request.Email}' - Code 400");
            return BadRequest();
        }

        string[] domainParts = domain.Split('.');
        string topLevelDomain = domainParts[^1];
        int minimalLengthPassword = 3;
        int minimalLengthDomain = 3;
        int maxNameLength = 255;
        int maxLengthDomain = 6;

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 3 || request.Name.Length > maxNameLength)  {
            _logger.LogError("Invalid name in signup request - Code 400");
            return BadRequest();
        } else if (_users.Any(a => a.Email == request.Email)) {
            _logger.LogError($"Duplicate email '{request.Email}' in signup - Code 400");
            return BadRequest();
        } else  if (domain.Length < minimalLengthDomain || topLevelDomain.Length < 2 || topLevelDomain.Length > maxLengthDomain) {
            _logger.LogError($"Invalid domain or TLD in email '{request.Email}' - Code 400");
            return BadRequest();
        } else if (!IsValidEmailParts(local, domain))  {
            _logger.LogError($"Invalid characters in email '{request.Email}' - Code 400");
            return BadRequest();
        } else if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < minimalLengthPassword) {
            _logger.LogError("Invalid password in signup request - Code 400");
            return BadRequest();
        }

        var user = new User() {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password)
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

    private static string HashPassword(string password) {
        // тимчасова версія;
        // поки просто кодуємо у Base64;
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }

    // Email validation
    private static bool IsValidEmailParts(string local, string domain) {
        if (string.IsNullOrWhiteSpace(local) || string.IsNullOrWhiteSpace(domain)) {
            return false;
        }

        // Перевірка локальної частини (перед @). 
        // (?!.*\.\.) - заборона двох крапок подряд
        // (?!.*--) - заборона двох дифісів подряд
        
        string localPattern = @"^(?!.*\.\.)(?!.*--)[a-zA-Z0-9._%+-]+$";
        if (!Regex.IsMatch(local, localPattern) || local.StartsWith('.') || local.StartsWith('-') || local.EndsWith('.') || local.EndsWith('-')) {
            return false;
        }

        // Перевірка доменної частини (після @)
        string domainPattern = @"^(?!.*\.\.)(?!.*--)[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(domain, domainPattern) || domain.StartsWith('.') || domain.StartsWith('-') || domain.EndsWith('.') || domain.EndsWith('-')) {
            return false;
        }

        return true;
    }

}