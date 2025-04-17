using Microsoft.AspNetCore.Mvc;
using LoggerNamespace;
using SothbeysKillerApi.Services;

namespace SothbeysKillerApi.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UserController : ControllerBase {
    private static List<User> _users = [];
    private readonly Logger _logger;
    private readonly IUserValidationService _validator;

    public UserController(Logger logger, IUserValidationService validator) {
        _logger = logger;
        _validator = validator;
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Signup(UserCreateRequest request) {
        if (!_validator.ValidateSignup(request, _users, out string errorMessage)) {
            _logger.LogError($"{errorMessage} - Code 400");
            return BadRequest();
        }

        var user = new User {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Email = request.Email,
            Password = HashPassword(request.Password)
        };

        _users.Add(user);
        return NoContent();
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Signin(UserSigninRequest request) {
        var user = _users.SingleOrDefault(a => a.Email == request.Email);

        if (user is null) {
            _logger.LogError($"User with email '{request.Email}' not found - Code 404");
            return NotFound();
        }
        if (!(user.Password.Equals(request.Password))) {
            _logger.LogError($"Invalid password for user '{request.Email}' - Code 401");
            return Unauthorized();
        }

        var response = new UserSigninResponse(user.Id, user.Name, user.Email);
        return Ok(response);
    }

    private static string HashPassword(string password) {
        return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
    }
}
