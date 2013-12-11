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
using CloudFox.Presentation.ViewModels;
using Ninject;
using CloudFox.Presentation.Util;
using System.Windows.Navigation;

namespace CloudFox.Presentation.Views
{
    public partial class SearchPage : PhoneApplicationPage
    {
        private bool isNewPage;

        public SearchPage()
        {
            InitializeComponent();

            DataContext = App.Kernel.Get<SearchViewModel>();

            // Give the focus to the search text box
            this.Loaded += new RoutedEventHandler((sender, e) => { searchTextBox.Focus(); });

            isNewPage = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (isNewPage)
            {
                TombstoneHelper.RestoreState(this);
                TombstoneHelper.RestoreState(this, searchResultsListbox);
                TombstoneHelper.RestoreState(this, searchTextBox);

                isNewPage = false;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            TombstoneHelper.SaveState(this);
            TombstoneHelper.SaveState(this, searchResultsListbox);
            TombstoneHelper.SaveState(this, searchTextBox);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendCurrentView();
        }
    }
}