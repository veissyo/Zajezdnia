using System.Security.Cryptography;
using System.Text;

namespace Zajezdnia.Services;

public class PasswordService
{
    private const int SaltSize = 16;
    private const int HashSize = 32;
    private const int Iterations = 100_000;

    public string HashHasla(string haslo)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(haslo), salt,
            Iterations, HashAlgorithmName.SHA256, HashSize);

        return $"v1:{Convert.ToBase64String(salt)}:{Convert.ToBase64String(hash)}";
    }

    public bool WeryfikujHaslo(string haslo, string zapisanyHash)
    {
        try
        {
            var parts = zapisanyHash.Split(':');
            if (parts.Length != 3 || parts[0] != "v1") return false;

            byte[] salt = Convert.FromBase64String(parts[1]);
            byte[] expected = Convert.FromBase64String(parts[2]);
            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(haslo), salt,
                Iterations, HashAlgorithmName.SHA256, HashSize);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
        catch { return false; }
    }
}