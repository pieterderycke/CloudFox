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

    public class DateTimeJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            double microseconds = (long)reader.Value / 1000.0;

            DateTime dateTime = new DateTime(1970, 1, 1);
            return dateTime.AddMilliseconds(microseconds);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DateTime dateTime = (DateTime)value;
            double microseconds = (dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds * 1000.0;
            writer.WriteValue(microseconds);
        }
    }
}
