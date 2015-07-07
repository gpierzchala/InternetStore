using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace DataAccess
{
    public class NHibernateConnection : INhibernateConnection
    {
        ISessionFactory INhibernateConnection.CreateSessionFactory()
        {
            const string connectionString = @"Data Source=STACJONARNY;Initial Catalog=SklepInternetowy;Integrated Security=True";
            //const string connectionString = @"Data Source=GRZESIEKLAPTOP\SQLSERVER;Initial Catalog=Shop;Integrated Security=True";
            //const string connectionString = @"TUTAJ WKLEJ CONNECTION STRING";
            return Fluently.Configure()
              .Database(MsSqlConfiguration.MsSql2012.ConnectionString(
                  connectionString))
              .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHibernateConnection>()
              )
              .BuildSessionFactory();
        }
    }
}
