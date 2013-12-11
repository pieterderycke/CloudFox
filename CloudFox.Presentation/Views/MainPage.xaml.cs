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
using Microsoft.Phone.Tasks;
using System.Windows.Navigation;
using Microsoft.Phone.Shell;
using CloudFox.Presentation.Util;

namespace CloudFox.Presentation.Views
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool isNewPage;

        // Constructor
        public MainPage()
        {
            InitializeComponent();

            DataContext = App.Kernel.Get<MainViewModel>();

            isNewPage = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Restore the Tombstoned State
            if (isNewPage)
            {
                TombstoneHelper.RestoreState(this);
                TombstoneHelper.RestoreState(this, categoryPivot);
                TombstoneHelper.RestoreState(this, bookmarkListBox);
                TombstoneHelper.RestoreState(this, historyListBox);
                TombstoneHelper.RestoreState(this, tabsListBox);

                isNewPage = false;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            TombstoneHelper.SaveState(this);
            TombstoneHelper.SaveState(this, categoryPivot);
            TombstoneHelper.SaveState(this, bookmarkListBox);
            TombstoneHelper.SaveState(this, historyListBox);
            TombstoneHelper.SaveState(this, tabsListBox);
        }

        private void SettingsMenuItem_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/Views/SettingsPage.xaml", UriKind.Relative));
        }

        private void AboutMenuItem_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/Views/AboutPage.xaml", UriKind.Relative));
        }

        private void ProfileMenuItem_Click(object sender, EventArgs e)
        {
            // Navigate to the new page
            NavigationService.Navigate(new Uri("/Views/ProfilePage.xaml", UriKind.Relative));
        }

        // Temporary until Prism supports WP 7.1
        private void Refresh_Click(object sender, EventArgs e)
        {
            ((MainViewModel)this.DataContext).Refresh.Execute(null);
        }

        // Temporary until Prism supports WP 7.1
        private void SelectFolder_Click(object sender, EventArgs e)
        {
            ((MainViewModel)this.DataContext).SelectFolder.Execute(null);
        }

        // Temporary until Prism supports WP 7.1
        private void Search_Click(object sender, EventArgs e)
        {
            ((MainViewModel)this.DataContext).Search.Execute(null);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendCurrentView();
        }
    }
}