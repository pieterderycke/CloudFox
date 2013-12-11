using System;
using Newtonsoft.Json;

namespace CloudFox.Weave.Tests
{
    public class JsonReaderMock : JsonReader
    {
        private object value;

        public JsonReaderMock(object value)
        {
            this.value = value;
        }

        public override object Value
        {
            get
            {
                return this.value;
            }
        }

        public override Type ValueType
        {
            get
            {
                return this.value.GetType();
            }
        }

        public override bool Read()
        {
            return true;
        }

        public override byte[] ReadAsBytes()
        {
            throw new NotImplementedException();
        }

        public override DateTimeOffset? ReadAsDateTimeOffset()
        {
            return DateTimeOffset.MinValue;
        }

        public override decimal? ReadAsDecimal()
        {
            return decimal.MinValue;
        }
    }
}
