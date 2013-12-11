using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Newtonsoft.Json;

namespace CloudFox.Weave.Util
{
    /// <summary>
    /// A <see cref="JsonConverter"/> implementation to convert the bookmark type to a .Net enum.
    /// </summary>
    public class BookmarkTypeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(BookmarkType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type type = reader.ValueType;

            if (type != typeof(string))
            {
                throw new InvalidOperationException("Cannot convert " + type.FullName + " to a " +
                    typeof(BookmarkType).Name + ".");
            }
            else
            {
                return Enum.Parse(typeof(BookmarkType), (string)reader.Value, true);
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
