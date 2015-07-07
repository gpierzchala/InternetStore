using DataAccess.Entities;

namespace SklepInternetowy.Helpers
{
    public class CryptoHelpers
    {
        public static bool IsValid(Users user, string password)
        {
            var isValid = false;
            var crypto = new SimpleCrypto.PBKDF2 {Salt = user.PasswordSalt};
            var enteredPassword = crypto.Compute(password);
            if (enteredPassword == user.Password)
                isValid = true;
            
            return isValid;
        }
    }
}