using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class ProductsMap : ClassMap<Products>
    {
        public ProductsMap()
        {
            Table("Products");

            Id(x => x.ID).Column("ID").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable();
            Map(x => x.Description).Column("Description").Not.Nullable();
            Map(x => x.Price).Column("Price").Not.Nullable();
            Map(x => x.Quantity).Column("Quantity").Not.Nullable();
            References(x => x.Category).Column("CategoryID").Not.Nullable();
            References(x => x.Manufacturer).Column("ManufacturerID").Not.Nullable();
            Map(x => x.IsFeatured).Column("IsFeatured").Not.Nullable();
            Map(x => x.IsRecent).Column("IsRecent").Not.Nullable();
            Map(x => x.IsBestSeller).Column("IsBestSeller").Not.Nullable();
            Map(x => x.ShortDescription).Column("ShortDescription").Nullable();
        }
    }
}