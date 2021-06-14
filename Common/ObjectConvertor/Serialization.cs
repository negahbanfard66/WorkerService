using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Common.ObjectConvertor
{
    public class Serialization
    {
        public static T FromByteArray<T>(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Binder = new StudentModelSerializationBinder();
            using (MemoryStream ms = new MemoryStream(data))
            {
                return (T)formatter.Deserialize(ms);
            }
        }

        public static byte[] ObjectToByteArray(Object obj)
        {
            BinaryFormatter bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
    }
}
