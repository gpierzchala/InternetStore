using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class DeliveryTypesMap : ClassMap<DeliveryTypes>
    {
        public DeliveryTypesMap()
        {
            Table("DeliveryTypes");
            Id(x => x.ID).Column("ID").Column("ID").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable();
            Map(x => x.Price).Column("Price").Not.Nullable();
        }
    }
}
