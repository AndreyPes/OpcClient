using Opc.Ua;
using System;

namespace OpcClient.Logic
{
    public class TypeConverter
    {

        public static object Convert(string type, string value)
        {
            if (type == null)
                return null;
            if (type != "text" && value == string.Empty)
                value = "0";
            switch (type.ToLowerInvariant())
            {
                case "long":
                    return Int32.Parse(value);
                case "text":
                    return value;
                case "single":
                    return float.Parse(value);
                case "yesno":
                    return bool.Parse(value);
                default:
                    return null;
            }
        }

        public static TypeInfo Convert(string type)
        {
            if (type == null)
                return null;
            switch (type.ToLowerInvariant())
            {
                case "long":
                    return TypeInfo.Scalars.Int32;
                case "text":
                    return TypeInfo.Scalars.String;
                case "single":
                    return TypeInfo.Scalars.Float;
                case "yesno":
                    return TypeInfo.Scalars.Boolean;
                default:
                    return null;
            }
        }
    }
}
