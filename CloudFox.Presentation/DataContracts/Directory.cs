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
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CloudFox.Presentation
{
    [DataContract]
    public class Directory
    {
        public Directory(string name, string id)
        {
            Name = name;
            Id = id;
            Bookmarks = new List<Bookmark>();
            Directories = new List<Directory>();
        }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Id { get; set; }

        public Directory Parent { get; set; }

        [DataMember]
        public IList<Bookmark> Bookmarks { get; set; }

        [DataMember]
        public IList<Directory> Directories { get; set; }
    }
}
