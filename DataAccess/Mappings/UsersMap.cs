using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class UsersMap : ClassMap<Users>
    {
        public UsersMap()
        {
            Table("Users");
            Id(x => x.ID).Not.Nullable().Column("ID").GeneratedBy.Guid();
            Map(x => x.Name).Not.Nullable().Column("Name");
            Map(x => x.Surname).Not.Nullable().Column("Surname");
            Map(x => x.Email).Not.Nullable().Column("Email");
            Map(x => x.Password).Not.Nullable().Column("Password");
            Map(x => x.City).Not.Nullable().Column("City");
            Map(x => x.Address).Not.Nullable().Column("Address");
            Map(x => x.ZipCode).Not.Nullable().Column("ZipCode");
            Map(x => x.IsAdmin).Not.Nullable().Column("IsAdmin");
            Map(x => x.PasswordSalt).Not.Nullable().Column("PasswordSalt");
            Map(x => x.IPAddress).Not.Nullable().Column("IPAddress");
        }
    }
}