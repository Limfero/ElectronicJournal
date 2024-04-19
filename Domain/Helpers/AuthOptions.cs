using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ElectronicJournal.Domain.Helpers
{
    public class AuthOptions
    {
        public const string ISSUER = "MyToken"; // издатель токена
        public const string AUDIENCE = "ElectronicJournal"; // потребитель токена
        const string KEY = "mysupersecret_secretsecretsecretkey!123"; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
