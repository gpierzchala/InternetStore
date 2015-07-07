namespace DataAccess.Entities
{
    public class Categories
    {
        protected Categories() { }

        public Categories(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual int ID { get; set; }
        public virtual string Name { get; set; }
        
        public virtual string Description { get; set; }

        public virtual void Update(string name, string description)
        {
            if (!string.IsNullOrEmpty(name) && Name != name)
                Name = name;
            if (Description != description)
                Description = description;
            
        }
    }
}
