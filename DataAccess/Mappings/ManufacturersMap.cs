using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class ManufacturersMap : ClassMap<Manufacturers>
    {
        public ManufacturersMap()
        {
            Table("Manufacturers");
            Id(x => x.ID).Column("ID").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable();
        }
    }
}
