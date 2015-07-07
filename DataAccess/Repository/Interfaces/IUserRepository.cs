using System;
using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IUserRepository
    {
        IList<Users> GetAllUsers();
        void Save(Users user);
        Users Get(Guid id);
        Users Get(string username);
        bool FindDuplicateByEmail(string email);
        void Delete(Users user);
        void Update(string userLogin,string name, string surname, string email, string city, string street, string zipCode);
        void ChangeUserRole(string email);
    }
}
