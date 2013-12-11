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
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace CloudFox.Presentation
{
    [DataContract]
    public class ProfileInformation
    {
        public ProfileInformation(string weaveNode, int serverStorageVersion,
            IEnumerable<CollectionInformation> collections, IEnumerable<Client> clients)
        {
            this.WeaveNode = weaveNode;
            this.ServerStorageVersion = serverStorageVersion;
            this.Collections = new List<CollectionInformation>(collections);
            this.Clients = new List<Client>(clients);
        }

        [DataMember]
        public string WeaveNode { get; set; }

        [DataMember]
        public int ServerStorageVersion { get; set; }

        [DataMember]
        public IList<CollectionInformation> Collections { get; set; }

        [DataMember]
        public IList<Client> Clients { get; set; }
    }
}
