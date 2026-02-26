using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enum
{
    [Serializable]
    public class EnumMap<TEnum, TValue> where TEnum : System.Enum
    {
        [FormerlySerializedAs("values")] [SerializeField] private TValue[] _values;

        public TValue this[TEnum key]
        {
            get => _values[Convert.ToInt32(key)];
            set => _values[Convert.ToInt32(key)] = value;
        }
    }
}
