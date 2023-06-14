using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Utils;

public static class HashPassword
{
    private const int SaltSize = 16; // Size of the salt in bytes
    private const int HashSize = 20; // Size of the hash in bytes
    private const int Iterations = 10000; // Number of iterations

    public static string Hash(this string password)
    {
        // Generate a random salt
        byte[] salt;
        new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);

        // Hash the password and combine it with the salt
        byte[] hash = PBKDF2(password, salt, Iterations, HashSize);

        // Combine the salt and hash into a single byte array
        byte[] hashBytes = new byte[SaltSize + HashSize];
        Array.Copy(salt, 0, hashBytes, 0, SaltSize);
        Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

        // Convert the combined bytes to a Base64-encoded string
        string base64Hash = Convert.ToBase64String(hashBytes);

        return base64Hash;
    }

    public static bool Verify(this string password, string hashedPassword)
    {
        // Convert the hashed password string to a byte array
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);

        // Extract the salt from the hashed password byte array
        byte[] salt = new byte[SaltSize];
        Array.Copy(hashBytes, 0, salt, 0, SaltSize);

        // Compute the hash of the input password using the extracted salt
        byte[] computedHash = PBKDF2(password, salt, Iterations, HashSize);

        // Compare the computed hash with the stored hash
        for (int i = 0; i < HashSize; i++)
        {
            if (hashBytes[i + SaltSize] != computedHash[i])
                return false;
        }

        return true;
    }

    private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
    {
        using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations))
        {
            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
