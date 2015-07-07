using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
   public class OrderStateMap : ClassMap<OrderState>
   {
       public OrderStateMap()
       {
           Table("OrderState");
           Id(x => x.ID).Column("ID").Not.Nullable();
           Map(x => x.StateName).Column("StateName").Not.Nullable();
       }
    }
}
