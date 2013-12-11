using System;
using System.Net;
using System.Runtime.Serialization;

namespace CloudFox.Presentation
{
    [DataContract]
    public class Client
    {
        public Client(string id, string name, string type)
        {
            this.Id = id;
            this.Name = name;
            this.Type = type;
        }

        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}
