using System.Security.Cryptography;
using System.Text;

namespace App.Helpers;

public static class PasswordHelper
{
    public static string GenerateSalt(int length = 32)
    {
        var buffer = RandomNumberGenerator.GetBytes(length);
        return Convert.ToBase64String(buffer);
    }

    public static string HashPassword(string password, string salt)
    {
        using var sha = SHA256.Create();
        var combined = Encoding.UTF8.GetBytes(password + salt);
        var hash = sha.ComputeHash(combined);
        return Convert.ToBase64String(hash);
    }
}
