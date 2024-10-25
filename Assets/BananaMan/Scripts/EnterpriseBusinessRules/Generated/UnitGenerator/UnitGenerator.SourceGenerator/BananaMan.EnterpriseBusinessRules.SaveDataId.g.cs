﻿// <auto-generated>
// THIS (.cs) FILE IS GENERATED BY UnitGenerator. DO NOT CHANGE IT.
// </auto-generated>
#pragma warning disable CS8669
using System;
using System.Globalization;
#if NET7_0_OR_GREATER
using System.Numerics;
#endif
using System.Text.Json;
using System.Text.Json.Serialization;
namespace BananaMan.EnterpriseBusinessRules
{
    [JsonConverter(typeof(SaveDataIdJsonConverter))]
    [System.ComponentModel.TypeConverter(typeof(SaveDataIdTypeConverter))]
    readonly partial struct SaveDataId 
        : IEquatable<SaveDataId>
        , IComparable<SaveDataId>
        , IFormattable
#if NET7_0_OR_GREATER
        , IComparisonOperators<SaveDataId, SaveDataId, bool>
#endif
#if NET8_0_OR_GREATER
        , IEqualityOperators<SaveDataId, SaveDataId, bool>
#endif
    {
        readonly int value;

        public int AsPrimitive() => value;

        public SaveDataId(int value)
        {
            this.value = value;
        }
        
        public static implicit operator int(SaveDataId value)
        {
            return value.value;
        }

        public static implicit operator SaveDataId(int value)
        {
            return new SaveDataId(value);
        }

        public bool Equals(SaveDataId other)
        {
            return value.Equals(other.value);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var t = obj.GetType();
            if (t == typeof(SaveDataId))
            {
                return Equals((SaveDataId)obj);
            }
            if (t == typeof(int))
            {
                return value.Equals((int)obj);
            }

            return value.Equals(obj);
        }
        
        public static bool operator ==(SaveDataId x, SaveDataId y)
        {
            return x.value.Equals(y.value);
        }

        public static bool operator !=(SaveDataId x, SaveDataId y)
        {
            return !x.value.Equals(y.value);
        }

        public override int GetHashCode()
        {
            return value.GetHashCode();
        }

        public override string ToString() => value.ToString();

        public string ToString(string? format, IFormatProvider? formatProvider) => value.ToString(format, formatProvider);

        // UnitGenerateOptions.Comparable

        public int CompareTo(SaveDataId other)
        {
            return value.CompareTo(other.value);
        }
        public static bool operator >(SaveDataId x, SaveDataId y)
        {
            return x.value > y.value;
        }

        public static bool operator <(SaveDataId x, SaveDataId y)
        {
            return x.value < y.value;
        }

        public static bool operator >=(SaveDataId x, SaveDataId y)
        {
            return x.value >= y.value;
        }

        public static bool operator <=(SaveDataId x, SaveDataId y)
        {
            return x.value <= y.value;
        }

        // UnitGenerateOptions.JsonConverter
        
        private class SaveDataIdJsonConverter : JsonConverter<SaveDataId>
        {
            public override void Write(Utf8JsonWriter writer, SaveDataId value, JsonSerializerOptions options)
            {
                var converter = options.GetConverter(typeof(int)) as JsonConverter<int>;
                if (converter != null)
                {
                    converter.Write(writer, value.value, options);
                }
                else
                {
                    throw new JsonException($"{typeof(int)} converter does not found.");
                }
            }

            public override SaveDataId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var converter = options.GetConverter(typeof(int)) as JsonConverter<int>;
                if (converter != null)
                {
                    return new SaveDataId(converter.Read(ref reader, typeToConvert, options));
                }
                else
                {
                    throw new JsonException($"{typeof(int)} converter does not found.");
                }
            }

        }

        // Default
        
        private class SaveDataIdTypeConverter : System.ComponentModel.TypeConverter
        {
            private static readonly Type WrapperType = typeof(SaveDataId);
            private static readonly Type ValueType = typeof(int);

            public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == WrapperType || sourceType == ValueType)
                {
                    return true;
                }

                return base.CanConvertFrom(context, sourceType);
            }

            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
            {
                if (destinationType == WrapperType || destinationType == ValueType)
                {
                    return true;
                }

                return base.CanConvertTo(context, destinationType);
            }

            public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
            {
                if (value != null)
                {
                    var t = value.GetType();
                    if (t == typeof(SaveDataId))
                    {
                        return (SaveDataId)value;
                    }
                    if (t == typeof(int))
                    {
                        return new SaveDataId((int)value);
                    }
                }

                return base.ConvertFrom(context, culture, value);
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
            {
                if (value is SaveDataId wrappedValue)
                {
                    if (destinationType == WrapperType)
                    {
                        return wrappedValue;
                    }

                    if (destinationType == ValueType)
                    {
                        return wrappedValue.AsPrimitive();
                    }
                }

                return base.ConvertTo(context, culture, value, destinationType);
            }
        }
    }
}
