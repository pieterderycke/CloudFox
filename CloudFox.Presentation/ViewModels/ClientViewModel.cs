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

namespace CloudFox.Presentation.ViewModels
{
    public class ClientViewModel
    {
        public ClientViewModel(Client client)
        {
            Text = string.Format("{0} ({1})", client.Name, client.Type);
        }

        public string Text { get; private set; }
    }
}
