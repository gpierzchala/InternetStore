using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class OrderDetailsMap : ClassMap<OrderDetails>
    {
        public OrderDetailsMap()
        {
            Table("OrderDetails");
            Id(x => x.ID).Column("ID").Not.Nullable();
            Map(x => x.Quantity).Column("Quantity").Not.Nullable();
            Map(x => x.UnitPrice).Column("UnitPrice").Not.Nullable();
            References(x => x.Product).Column("ProductID").Not.Nullable();
            References(x => x.Order).Column("OrderId").Not.Nullable();
        }
    }
}
