using System.Security.Cryptography;
using System.Text;

namespace ElectronicJournal.Domain.Helpers
{
    public class HashPasswordHelper
    {
        public static string HashPassword(string password)
        {

            var hashedByts = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedByts).Replace("-", "").ToLower();

            return hash;
        }
    }
}
