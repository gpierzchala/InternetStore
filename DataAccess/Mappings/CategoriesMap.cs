using DataAccess.Entities;
using FluentNHibernate.Mapping;

namespace DataAccess.Mappings
{
    public class CategoriesMap : ClassMap<Categories>
    {
        public CategoriesMap()
        {
            Table("Categories");
            Id(x => x.ID).Column("ID").Not.Nullable();
            Map(x => x.Name).Column("Name").Not.Nullable();
            Map(x => x.Description).Column("Description").Nullable();
        }
    }
}
