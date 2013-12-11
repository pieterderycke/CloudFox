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
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using CloudFox.Presentation.Util;

namespace CloudFox.Presentation.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private IStorage storage;
        private INavigationService navigationService;

        public SettingsViewModel(IStorage  storage, INavigationService navigationService)
        {
            this.storage = storage;
            this.navigationService = navigationService;

            this.UserName = storage.UserName;
            this.Password = storage.Password;
            this.Passphrase = storage.Passphrase;
            this.UseDefaultServer = storage.UseDefaultServer;
            this.ServerAddress = storage.ServerAddress;
            this.SynchronizeBookmarks = storage.SynchronizeBookmarks;
            this.SynchronizeHistory = storage.SynchronizeHistory;
            this.SynchronizeTabs = storage.SynchronizeTabs;
            this.Save = new RelayCommand(() => SaveToStorage());
            this.Cancel = new RelayCommand(() => this.navigationService.GoBack());
            this.Help = new RelayCommand(() => this.navigationService.GoToPage("SettingsHelpPage.xaml"));
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Passphrase { get; set; }

        public bool UseDefaultServer { get; set; }

        public string ServerAddress { get; set; }

        public bool SynchronizeBookmarks { get; set; }

        public bool SynchronizeHistory { get; set; }

        public bool SynchronizeTabs { get; set; }

        public ICommand Save { get; private set; }

        public ICommand Cancel { get; private set; }

        public ICommand Help { get; private set; }

        private void SaveToStorage()
        {
            storage.UserName = UserName;
            storage.Password = Password;
            storage.Passphrase = Passphrase;
            storage.UseDefaultServer = UseDefaultServer;
            storage.ServerAddress = ServerAddress;
            storage.SynchronizeBookmarks = SynchronizeBookmarks;
            storage.SynchronizeHistory = SynchronizeHistory;
            storage.SynchronizeTabs = SynchronizeTabs;

            storage.Persist();

            navigationService.GoBack();
        }
    }
}
