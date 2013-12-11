using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Ninject;
using CloudFox.Presentation.ViewModels;
using CloudFox.Presentation.Util;

namespace CloudFox.Presentation.Views
{
    public partial class ProfilePage : PhoneApplicationPage
    {
        public ProfilePage()
        {
            InitializeComponent();

            DataContext = App.Kernel.Get<ProfileViewModel>();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendCurrentView();
        }
    }
}