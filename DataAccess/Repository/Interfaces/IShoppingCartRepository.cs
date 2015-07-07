using System.Collections.Generic;
using DataAccess.Entities;

namespace DataAccess.Repository.Interfaces
{
    public interface IShoppingCartRepository
    {
        ShoppingCarts Get(int id);
        IList<ShoppingCarts> GetAll();
        void Save(ShoppingCarts entity);
        void Update(ShoppingCarts cartItem);
        void Delete(ShoppingCarts cartItem);
    }
}
