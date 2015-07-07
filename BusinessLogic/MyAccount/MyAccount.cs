using BusinessLogic.Models;
using DataAccess.Entities;
using DataAccess.Repository.Interfaces;

namespace BusinessLogic.MyAccount
{
    public class MyAccount
    {
        private readonly IUserRepository _userRepo;

        public MyAccount(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public Users GetUser(string username)
        {
            return _userRepo.Get(username);
        }

        public bool FindDuplicateUser(string username)
        {
            return _userRepo.FindDuplicateByEmail(username);
        }

        public bool IsSuccessedCreatedUser(RegisterModel model, string enryptPass, string address, bool isAdmin,
            string passwordSalt, string ipaddress)
        {
            var user = new Users(model.Name, model.Surname, model.Email, enryptPass, model.City, address, model.ZipCode,
                isAdmin, passwordSalt,
                ipaddress);
            try
            {
                _userRepo.Save(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void ChangePassword(string userEmail, string encryptedPassword, string salt)
        {
            var user = _userRepo.Get(userEmail);
            user.Password = encryptedPassword;
            user.PasswordSalt = salt;
            _userRepo.Save(user);
        }
    }
}