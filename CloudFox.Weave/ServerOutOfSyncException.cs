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

namespace CloudFox.Weave
{
    public class ServerOutOfSyncException : Exception
    {
        public ServerOutOfSyncException(string message)
            : base(message)
        {
        }

        public ServerOutOfSyncException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
