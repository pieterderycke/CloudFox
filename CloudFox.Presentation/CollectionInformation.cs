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

namespace CloudFox.Presentation
{
    [DataContract]
    public class CollectionInformation
    {
        public CollectionInformation(string name, int itemCount, int size)
        {
            this.Name = name;
            this.ItemCount = itemCount;
            this.Size = size;
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int ItemCount { get; set; }

        [DataMember]
        public int Size { get; set; }
    }
}
