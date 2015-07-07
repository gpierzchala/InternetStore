using System;

namespace DataAccess.Entities
{
    public class Orders
    {
        protected Orders()
        {
        }

        public Orders(Users user, DateTime orderDate, decimal summaryPrice, DeliveryTypes deliveryType, OrderState orderState)
        {
            User = user;
            OrderDate = orderDate;
            SummaryPrice = summaryPrice;
            DeliveryType = deliveryType;
            State = orderState;
        }

        public virtual int Id { get; set; }
        public virtual Users User { get; set; }
        public virtual DateTime OrderDate { get; set; }
        public virtual decimal SummaryPrice { get; set; }
        public virtual DeliveryTypes DeliveryType { get; set; }
        public virtual OrderState State { get; set; }
    }
}