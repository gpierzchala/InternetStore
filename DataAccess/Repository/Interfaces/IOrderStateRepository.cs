using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IOrderStateRepository
    {
        OrderState Get(int id);
        void Save(OrderState newOrderState);
        IList<OrderState> GetAll();
        void Delete(OrderState orderState);
    }
}
