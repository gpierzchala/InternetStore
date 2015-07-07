using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class ShoppingCartsMap : ClassMap<ShoppingCarts>
    {
        public ShoppingCartsMap()
        {
            Table("ShoppingCarts");
            Id(x => x.ID).Column("ID").Not.Nullable();
            Map(x => x.Quantity).Column("Quantity").Not.Nullable();
            Map(x => x.DateCreated).Column("DateCreated").Not.Nullable();
            Map(x => x.CartId).Column("CartId").Not.Nullable();
            References(x => x.Product).Column("ProductId").Not.Nullable();
        }
    }
}
