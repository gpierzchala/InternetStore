using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IOrderDetailsRepository
    {
        IList<OrderDetails> GetAll();
        void Save(OrderDetails entity);
        OrderDetails Get(int id);

        IList<OrderDetails> GetForOrder(int orderId);
        void Delete(OrderDetails entity);
    }
}
