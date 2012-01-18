using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace ArtOfNet.ConfArt.Core
{
    public class DynamicConvertibleBase: DynamicObject, IConvertible
    {
        public virtual Func<string> GetValue { get; set; }

        private string TryGetValue()
        {
            string result;
            if (GetValue == null)
            {
                result = string.Empty; var x = new ExpandoObject();
            }
            else
            {
                try
                {
                    result = GetValue();
                }
                catch
                {
                    result = string.Empty;
                }
            }
            return result;
        }
        #region IConvertible
        public TypeCode GetTypeCode()
        {
            return TypeCode.String;
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            bool outValue;
            bool.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public byte ToByte(IFormatProvider provider)
        {
            byte outValue;
            byte.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public char ToChar(IFormatProvider provider)
        {
            char outValue;
            char.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            DateTime outValue;
            DateTime.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            decimal outValue;
            decimal.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public double ToDouble(IFormatProvider provider)
        {
            double outValue;
            double.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public short ToInt16(IFormatProvider provider)
        {
            Int16 outValue;
            Int16.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public int ToInt32(IFormatProvider provider)
        {
            Int32 outValue;
            Int32.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public long ToInt64(IFormatProvider provider)
        {
            Int64 outValue;
            Int64.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            sbyte outValue;
            sbyte.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public float ToSingle(IFormatProvider provider)
        {
            float outValue;
            float.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public string ToString(IFormatProvider provider)
        {
            return TryGetValue();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return (object)TryGetValue();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            ushort outValue;
            ushort.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            uint outValue;
            uint.TryParse(TryGetValue(), out outValue);
            return outValue;
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            ulong outValue;
            ulong.TryParse(TryGetValue(), out outValue);
            return outValue;
        }
        #endregion
    }
}
