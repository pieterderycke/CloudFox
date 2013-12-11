using System;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using CloudFox.Weave;
using GalaSoft.MvvmLight.Command;
using System.Threading;
using System.Windows.Threading;
using CloudFox.Presentation.Util;
using Microsoft.Phone.Tasks;
using GalaSoft.MvvmLight;
using CloudFox.Presentation.Localization;

namespace CloudFox.Presentation.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IStorage storage;
        private ISynchronizer synchronizer;
        private IMessageService messageService;
        private INavigationService navigationService;

        private Directory currentBookmarkDirectory;
        private Directory currentHistoryDirectory;
        private Directory currentTabsDirectory;
        private MainViewCategory category;

        private bool refreshInProgress;

        public MainViewModel(IStorage storage, ISynchronizer synchronizer, IMessageService messageService, INavigationService navigationService)
        {
            this.storage = storage;
            this.synchronizer = synchronizer;
            this.messageService = messageService;
            this.navigationService = navigationService;

            Bookmarks = new ObservableCollection<BookmarkViewModel>();
            History = new ObservableCollection<BookmarkViewModel>();
            Tabs = new ObservableCollection<BookmarkViewModel>();

            Refresh = new RelayCommand(RefreshAsync);
            SelectFolder = new RelayCommand(SelectFolderForCurrentCategory);
            Search = new RelayCommand(() => navigationService.GoToPage("SearchPage.xaml"));

            // Listen to events of the synchronizer component
            this.synchronizer.RefreshWeaveDataCompleted += new RefreshWeaveDataCompletedEventHandler(RefreshCompleted);
            this.synchronizer.RefreshWeaveDataProgressChanged += new ProgressChangedEventHandler(RefreshProgressChanged);

            // Load the bookmarks, history and opened tabs from durable storage
            this.storage.LoadBookmarks();
            this.storage.LoadHistory();
            this.storage.LoadTabs();
            UpdateData();
        }

        public MainViewCategory Category 
        {
            get
            {
                return this.category;
            }
            set
            {
                if (this.category != value)
                {
                    this.category = value;

                    RaisePropertyChanged("Category");
                    RaisePropertyChanged("Title");
                }
            }
        }

        public ObservableCollection<BookmarkViewModel> Bookmarks { get; private set; }
        public ObservableCollection<BookmarkViewModel> History { get; private set; }
        public ObservableCollection<BookmarkViewModel> Tabs { get; private set; }

        public BookmarkViewModel SelectedBookmark
        {
            get
            {
                return null; // noting selected by default
            }
            set
            {
                if (value != null)
                {
                    WebBrowserTask task = new WebBrowserTask();
                    task.Uri = new Uri(value.Location);
                    task.Show();
                }
            }
        }

        public BookmarkViewModel SelectedHistoryItem
        {
            get
            {
                return null; // noting selected by default
            }
            set
            {
                if (value != null)
                {
                    WebBrowserTask task = new WebBrowserTask();
                    task.Uri = new Uri(value.Location);
                    task.Show();
                }
            }
        }

        public BookmarkViewModel SelectedTab
        {
            get
            {
                return null; // noting selected by default
            }
            set
            {
                if (value != null)
                {
                    WebBrowserTask task = new WebBrowserTask();
                    task.Uri = new Uri(value.Location);
                    task.Show();
                }
            }
        }

        /// <summary>
        /// Gets or sets the the bookmark directory currently shown.
        /// </summary>
        [Tombstone]
        public Directory CurrentBookmarkDirectory
        {
            get
            {
                return this.currentBookmarkDirectory;
            }
            set
            {
                if (value != this.currentBookmarkDirectory)
                {
                    this.currentBookmarkDirectory = value;

                    // Update the collection
                    Bookmarks.Clear();

                    if(currentBookmarkDirectory != null)
                    {
                        IEnumerable<Bookmark> bladwijzerMenu = currentBookmarkDirectory.Bookmarks;

                        foreach (Bookmark bookmark in bladwijzerMenu)
                            Bookmarks.Add(new BookmarkViewModel(bookmark));
                    }

                    RaisePropertyChanged("Title");
                }
            }
        }

        [Tombstone]
        public Directory CurrentHistoryDirectory
        {
            get
            {
                return this.currentHistoryDirectory;
            }
            set
            {
                if (value != this.currentHistoryDirectory)
                {
                    this.currentHistoryDirectory = value;

                    // Update the collection
                    History.Clear();

                    if (currentHistoryDirectory != null)
                    {
                        IEnumerable<Bookmark> history = currentHistoryDirectory.Bookmarks;

                        foreach (Bookmark bookmark in history)
                            History.Add(new BookmarkViewModel(bookmark));
                    }
                }
            }
        }

        [Tombstone]
        public Directory CurrentTabsDirectory
        {
            get
            {
                return currentTabsDirectory;
            }
            set
            {
                if (value != this.currentTabsDirectory)
                {
                    this.currentTabsDirectory = value;

                    // Update the collection
                    Tabs.Clear();

                    if (currentHistoryDirectory != null)
                    {
                        IEnumerable<Bookmark> tabs = currentTabsDirectory.Bookmarks;

                        foreach (Bookmark bookmark in tabs)
                            Tabs.Add(new BookmarkViewModel(bookmark));
                    }
                }
            }
        }

        public string Title
        {
            get
            {
                switch (Category)
                {
                    case MainViewCategory.Bookmarks:
                        if (CurrentBookmarkDirectory != null)
                            return string.Format("{0} - {1}", CurrentBookmarkDirectory.Name.ToUpperInvariant(), App.ApplicationTitle);
                        break;
                    case MainViewCategory.History:
                        if (CurrentHistoryDirectory != null)
                            return string.Format("{0} - {1}", CurrentHistoryDirectory.Name.ToUpperInvariant(), App.ApplicationTitle);
                        break;
                    case MainViewCategory.Tabs:
                        if (CurrentTabsDirectory != null)
                            return string.Format("{0} - {1}", CurrentTabsDirectory.Name.ToUpperInvariant(), App.ApplicationTitle);
                        break;
                }

                // Default application title
                return App.ApplicationTitle;
            }
        }

        public ICommand Refresh { get; private set; }
        public ICommand SelectFolder { get; private set; }
        public ICommand Search { get; private set; }

        private void RefreshAsync()
        {
            if (!this.refreshInProgress)
            {
                this.refreshInProgress = true;
                this.synchronizer.RefreshWeaveDataAsync(Guid.NewGuid());
            }
        }

        private void RefreshCompleted(object sender, RefreshWeaveDataCompletedEventArgs e)
        {
            Exception ex = e.Error;

            if (ex == null)
            {
                UpdateData();

                this.messageService.ShowProgressMessage(AppResources.SynchronizationCompleted, false);
                this.messageService.StopProgressMessage(1000); // Stop with a delay of 1 second
            }
            else
            {
                this.messageService.StopProgressMessage(); // Stop imediatly

                if (ex is DataVerificationException)
                {
                    this.messageService.ShowErrorMessage(AppResources.UnableToDecryptError);
                }
                else
                {
                    this.messageService.ShowErrorMessage(ex.Message);
                }
            }

            this.refreshInProgress = false;
        }

        private void RefreshProgressChanged(RefreshWeaveDataProgressChangedEventArgs e)
        {
            this.messageService.ShowProgressMessage(e.Message, true);
        }

        private void UpdateData()
        {
            if (storage.Bookmarks != null && storage.Bookmarks.Directories.Count > 1)
            {
                CurrentBookmarkDirectory = storage.Bookmarks.Directories[1]; // the 2nd Directory is the bookmark menu
            }

            if (storage.History != null && storage.History.Directories.Count > 0)
            {
                CurrentHistoryDirectory = storage.History.Directories[0]; 
            }

            if (storage.Tabs != null && storage.Tabs.Directories.Count > 0)
            {
                CurrentTabsDirectory = storage.Tabs.Directories[0];
            }
        }

        private void SelectFolderForCurrentCategory()
        { 
            switch(Category)
            {
                case MainViewCategory.Tabs:
                    if (storage.Tabs == null || storage.Tabs.Directories.Count == 0)
                    {
                        messageService.ShowErrorMessage(AppResources.NoTabsAvailable);
                        return;
                    }
                    break;
                case MainViewCategory.History:
                    if (storage.History == null || storage.History.Directories.Count == 0)
                    {
                        messageService.ShowErrorMessage(AppResources.NoHistoryAvailable);
                        return;
                    }
                    break;
            }

            navigationService.GoToPage("SelectDirectoryPage.xaml");
        }
    }
}