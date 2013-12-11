using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CloudFox.Weave.Tests
{
    public class JsonWriterMock : JsonWriter
    {
        private Dictionary<Type, object> onWriteActions;

        public JsonWriterMock()
        {
            onWriteActions = new Dictionary<Type, object>();
        }

        public void OnWriteValue<T>(Action<T> assertAction)
        {
            onWriteActions.Add(typeof(T), assertAction);
        }

        public override void WriteValue(double value)
        {
            if (onWriteActions.ContainsKey(typeof(double)))
            {
                Action<double> onWriteAction = (Action<double>)onWriteActions[typeof(double)];
                onWriteAction(value);
            }
        }

        public override void Flush()
        {
        }
    }
}
