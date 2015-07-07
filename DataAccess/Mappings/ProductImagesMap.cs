using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class ProductImagesMap : ClassMap<ProductImages>
    {
        public ProductImagesMap()
        {
            Table("ProductImages");
            Id(x => x.ID).Not.Nullable().Column("ID");
            Map(x => x.ImageName).Not.Nullable().Column("ImageName");
            Map(x => x.ImageBytes).CustomSqlType("varbinary(MAX)").Length(2147483647).Not.Nullable().Column("ImageBytes");
            References(x => x.Product).Not.Nullable().Column("ProductID");
        }
    }
}
