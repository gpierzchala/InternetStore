using System.ComponentModel.DataAnnotations;

namespace DataAccess.Entities
{
    public class Manufacturers
    {
        protected Manufacturers() { }

        public Manufacturers(string name)
        {
            Name = name;
        }

        public virtual int ID { get; set; }
         public virtual string Name { get; set; }

        public virtual void Update(string name)
        {
            if (Name != null && Name != name)
            {
                Name = name;
            }
        }
    }
}
