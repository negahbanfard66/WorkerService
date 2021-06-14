using System;

namespace Data.Entity.Entities
{
    public class Student
    {
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Family
        {
            get;
            set;
        }

        public string Tell
        {
            get;
            set;
        }

        public string Address
        {
            get;
            set;
        }

        public DateTime InsertDate
        {
            get; 
            set;
        }

        public DateTime UpdateDate
        {
            get;
            set;
        }

    }
}
