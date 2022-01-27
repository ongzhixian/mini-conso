using System.Diagnostics.CodeAnalysis;

namespace Conso.Models;

[ExcludeFromCodeCoverage]
public class UserCredentialSetting
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public void EnsureIsValid()
    {
        if (string.IsNullOrWhiteSpace(Username)) {
            throw new InvalidDataException("Invalid username");
        }

        if (string.IsNullOrWhiteSpace(Password)) {
            throw new InvalidDataException("Invalid Password");
        }
    }

}
