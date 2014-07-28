using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Common
{
    /// <summary>
    /// 静态缓存
    /// </summary>
    /// <typeparam name="KeyType"></typeparam>
    /// <typeparam name="ValueType"></typeparam>
    public static class StaticCache<KeyType, ValueType>
    {
        public delegate ValueType GetValue(KeyType key);
        private static Dictionary<KeyType, ValueType> data = new Dictionary<KeyType, ValueType>();
        private static readonly object syncRoot = new object();
        public static ValueType Get(KeyType key, GetValue get)
        {
            if (!data.ContainsKey(key))
            {
                lock (syncRoot)
                {
                    ValueType value = get(key);
                    if (!data.ContainsKey(key))
                        data.Add(key, value);
                }

            }
            return data[key];
        }

        public static ValueType GetNew(KeyType key, GetValue get)
        {
            ValueType value = get(key);
            if (data.ContainsKey(key))
                data.Remove(key);
            data.Add(key, value);
            return value;
        }

        public static void Clear()
        {
            data.Clear();
        }
    }
}
