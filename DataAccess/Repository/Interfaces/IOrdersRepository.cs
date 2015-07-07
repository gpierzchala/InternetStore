using System;
using System.Collections.Generic;
using DataAccess.Entities;


namespace DataAccess.Repository.Interfaces
{
    public interface IOrdersRepository
    {
        Orders Get(int id);
        IList<Orders> GetAll();
        IList<Orders> GetAll(Guid userId); 
        void Save(Orders entity);
        void Update(Orders entity);
        void Delete(Orders entity);
        bool UpdateState(int orderId, int newState);
        IList<Orders> GetUserOrders(string email);
    }
}
