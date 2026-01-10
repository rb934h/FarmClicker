using System;
using UnityEngine;

namespace Enum
{
    [Serializable]
    public class EnumMap<TEnum, TValue> where TEnum : System.Enum
    {
        [SerializeField] private TValue[] values;

        public TValue this[TEnum key]
        {
            get => values[Convert.ToInt32(key)];
            set => values[Convert.ToInt32(key)] = value;
        }
    }
}