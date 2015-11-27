using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    [Serializable]
    public struct KeyValuePair<K, V>
    {
        public KeyValuePair(K key, V value)
            : this()
        {
            Key = key;
            Value = value;
        }

        public K Key { get; set; }
        public V Value { get; set; }

        public int comprobarKey(string key)
        {
            if (key.Equals(Key))
            {
                return 1;
            }
            return -1;
            
        }
    }
}
