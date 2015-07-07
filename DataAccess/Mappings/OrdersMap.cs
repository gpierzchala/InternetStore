using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class OrdersMap : ClassMap<Orders>
    {
        public OrdersMap()
        {
            Table("Orders");
            Id(x => x.Id).Column("ID").Not.Nullable();
            Map(x => x.OrderDate).Column("OrderDate").Not.Nullable();
            Map(x => x.SummaryPrice).Column("SummaryPrice").Not.Nullable();
            References(x => x.User).Column("UserId").Not.Nullable();
            References(x => x.DeliveryType).Column("DeliveryTypeId").Not.Nullable();
            References(x => x.State).Column("OrderStateId").Not.Nullable();
        }
    }
}