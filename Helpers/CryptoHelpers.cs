
using DataAccess.Entities;

namespace SklepInternetowy.Helpers
{
    public class CryptoHelpers
    {
        public static bool IsValid(Users user, string password)
        {
            bool isValid = false;
            var crypto = new SimpleCrypto.PBKDF2();

            if (user.Password == crypto.Compute(password,user.PasswordSalt))
            {
                isValid = true;
            }
            return isValid;
        }
    }
}