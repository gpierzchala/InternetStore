namespace DataAccess.Entities
{
    public class OrderState
    {
        protected OrderState() { }

        public OrderState(int id, string stateName)
        {
            ID = id;
            StateName = stateName;
        }

        public OrderState(string name)
        {
            StateName = name;
        }

        public virtual int ID { get; set; }
        public virtual string StateName { get; set; }
    }
}
