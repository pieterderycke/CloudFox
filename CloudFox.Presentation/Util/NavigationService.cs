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
using Microsoft.Phone.Controls;

namespace CloudFox.Presentation.Util
{
    public class NavigationService : INavigationService
    {
        public void GoBack()
        {
            ApplicationFrame.GoBack();
        }

        public void GoToPage(string page)
        {
            Uri pageUri = new Uri(string.Format("/Views/{0}", page), UriKind.Relative);
            ApplicationFrame.Navigate(pageUri);
        }

        private PhoneApplicationFrame ApplicationFrame
        {
            get
            {
                return (PhoneApplicationFrame)Application.Current.RootVisual;
            }
        }
    }
}
