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
using System.Windows.Navigation;
using CloudFox.Presentation.Util;

namespace CloudFox.Presentation.Views
{
    public partial class SelectDirectoryPage : PhoneApplicationPage
    {
        public SelectDirectoryPage()
        {
            InitializeComponent();

            DataContext = App.Kernel.Get<SelectDirectoryViewModel>();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            TombstoneHelper.RestoreState(this, directoyListBox);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            TombstoneHelper.SaveState(this, directoyListBox);
        }
    }
}