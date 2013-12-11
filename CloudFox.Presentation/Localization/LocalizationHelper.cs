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

namespace CloudFox.Presentation.Localization
{
    public class LocalizationHelper
    {
        private AppResources appResources = new AppResources();

        public AppResources AppResources 
        {
            get
            {
                return appResources;
            }
        }
    }
}
