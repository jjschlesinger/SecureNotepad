using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace SecureNotepad.Core.Extensions
{
    public static class SerializerExtensions
    {
        public static byte[] SerializeToBytes<T>(this T obj) where T : class
        {
            if (obj == null)
                return new byte[0];

            using (var ms = new MemoryStream())
            {
                var f = new BinaryFormatter();
                f.Serialize(ms, obj);
                ms.Position = 0;
                var b = new byte[ms.Length];
                ms.Read(b, 0, b.Length);
                return b;
            }
        }

        public static T DeserializeFromBytes<T>(this byte[] b) where T : class
        {
            if (b.Length == 0)
                return null;

            using (var ms = new MemoryStream(b))
            {
                var f = new BinaryFormatter();
                var obj = (T)f.Deserialize(ms);

                return obj;
            }
        }
    }
}
