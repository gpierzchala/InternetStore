using NHibernate;

namespace DataAccess
{
    public interface INhibernateConnection
    {
        ISessionFactory CreateSessionFactory();
    }
}
