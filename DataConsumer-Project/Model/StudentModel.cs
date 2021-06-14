using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConsumer_Project.Model
{
    [Serializable]
    public class StudentModel
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
