using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloudFox.Weave.Tests
{
    public class CommunicationChannelMock : ICommunicationChannel
    {
        private Dictionary<Tuple<string, RestOperation>, string> responses;

        public CommunicationChannelMock()
        {
            responses = new Dictionary<Tuple<string, RestOperation>, string>();
        }

        public bool IsInitialized { get; private set; }

        /// <summary>
        /// Add a new response to mock for a given url and rest operation. The payload
        /// is the payload that must be encapsulated in a <see cref="WeaveBasicObject"/> instance.
        /// This instance will be automatically created by this method.
        /// </summary>
        /// <param name="url">The url for the response.</param>
        /// <param name="operation">The rest operation for the response.</param>
        /// <param name="responsePayload">The data that will be returned back as response for the given url and restoperation.</param>
        public void AddResponse(string url, RestOperation operation, object responsePayload)
        {
            WeaveBasicObject wbo = new WeaveBasicObject();
            wbo.Id = Guid.NewGuid().ToString();
            wbo.ParentId = string.Empty;
            wbo.PredecessorId = string.Empty;
            wbo.Modified = DateTime.Now;
            wbo.Payload = JsonConvert.SerializeObject(responsePayload);

            AddResponse(url, operation, JsonConvert.SerializeObject(wbo));
        }

        public void AddResponse(string url, RestOperation operation, string response)
        {
            Tuple<string, RestOperation> key = new Tuple<string, RestOperation>(url, operation);
            responses.Add(key, response);
        }

        public void Initialize(string userName, string password)
        {
            IsInitialized = true;
        }

        public string Execute(string url, RestOperation operation)
        {
            Tuple<string, RestOperation> key = new Tuple<string, RestOperation>(url, operation);
            return responses[key];
        }

        public string Execute(string url, RestOperation operation, string payload)
        {
            throw new NotImplementedException();
        }
    }
}
