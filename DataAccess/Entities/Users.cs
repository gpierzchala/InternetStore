using System;

namespace DataAccess.Entities
{
    public class Users
    {
        protected Users()
        {
        }

        public Users(
            string name, string surname, string email, string password, string city, string address,
            string zip, bool isAdmin, string passwordSalt, string ipAddress)
        {
            Name = name;
            Surname = surname;
            Email = email;
            Password = password;
            Address = address;
            ZipCode = zip;
            IsAdmin = isAdmin;
            PasswordSalt = passwordSalt;
            City = city;
            IPAddress = ipAddress;
        }

        public virtual Guid ID { get; set; }

        public virtual string Name { get; set; }

        public virtual string Surname { get; set; }

        public virtual string Email { get; set; }

        public virtual string Password { get; set; }

        public virtual string City { get; set; }

        public virtual string Address { get; set; }
        public virtual string ZipCode { get; set; }
        public virtual bool IsAdmin { get; set; }
        public virtual string PasswordSalt { get; set; }
        public virtual string IPAddress { get; set; }
    }
}