using System.Text.RegularExpressions;
using SothbeysKillerApi.Controllers;

namespace SothbeysKillerApi.Services;

public class UserValidationService : IUserValidationService {
    public bool ValidateSignup(UserCreateRequest request, List<User> existingUsers, out string errorMessage) {
        errorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(request.Email)) {
            errorMessage = "Empty email";
            return false;
        }

        int atIndex = request.Email.IndexOf('@');
        if (atIndex <= 0 || atIndex == request.Email.Length - 1) {
            errorMessage = "Email missing '@' or domain";
            return false;
        }

        string local = request.Email[..atIndex];
        string domain = request.Email[(atIndex + 1)..];

        if (!domain.Contains('.')) {
            errorMessage = "Invalid domain in email";
            return false;
        }

        string[] domainParts = domain.Split('.');
        string topLevelDomain = domainParts[^1];

        int minimalLengthPassword = 3;
        int minimalLengthDomain = 3;
        int maxNameLength = 255;
        int maxLengthDomain = 6;

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 3 || request.Name.Length > maxNameLength) {
            errorMessage = "Invalid name";
            return false;
        }
        if (existingUsers.Any(u => u.Email == request.Email)) {
            errorMessage = "Duplicate email";
            return false;
        }
        if (domain.Length < minimalLengthDomain || topLevelDomain.Length < 2 || topLevelDomain.Length > maxLengthDomain) {
            errorMessage = "Invalid domain or TLD";
            return false;
        }
        if (!IsValidEmailParts(local, domain)) {
            errorMessage = "Invalid characters in email";
            return false;
        }
        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < minimalLengthPassword) {
            errorMessage = "Invalid password";
            return false;
        }

        return true;
    }

    public bool IsValidEmailParts(string local, string domain) {
        if (string.IsNullOrWhiteSpace(local) || string.IsNullOrWhiteSpace(domain)) {
            return false;
        }

        string localPattern = @"^(?!.*\.\.)(?!.*--)[a-zA-Z0-9._%+-]+$";
        if (!Regex.IsMatch(local, localPattern) || local.StartsWith('.') || local.StartsWith('-') || local.EndsWith('.') || local.EndsWith('-')) {
            return false;
        }

        string domainPattern = @"^(?!.*\.\.)(?!.*--)[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        if (!Regex.IsMatch(domain, domainPattern) || domain.StartsWith('.') || domain.StartsWith('-') || domain.EndsWith('.') || domain.EndsWith('-')) {
            return false;
        }

        return true;
    }
}
