using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using GalaSoft.MvvmLight;
using CloudFox.Presentation.Util;
using System.Windows;

namespace CloudFox.Presentation.ViewModels
{
    public class SelectDirectoryViewModel : ViewModelBase
    {
        private MainViewCategory Category;

        private IStorage storage;
        private INavigationService navigationService;
        private MainViewModel mainViewModel;

        public SelectDirectoryViewModel(MainViewModel mainViewModel, IStorage storage, INavigationService navigationService)
        {
            this.mainViewModel = mainViewModel;
            this.storage = storage;
            this.navigationService = navigationService;
            this.Directories = new ObservableCollection<DirectoryViewModel>();
            this.Category = mainViewModel.Category;

            // Load the bookmarks from durable storage
            this.storage.LoadBookmarks();
            UpdateDirectories();
        }

        public ObservableCollection<DirectoryViewModel> Directories { get; private set; }

        public DirectoryViewModel SelectedDirectory
        {
            get
            {
                return null; // noting selected by default
            }
            set
            {
                switch(Category)
                {
                    case MainViewCategory.Bookmarks:
                        this.mainViewModel.CurrentBookmarkDirectory = value.Directory;
                        break;
                    case MainViewCategory.History:
                        this.mainViewModel.CurrentHistoryDirectory = value.Directory;
                        break;
                    case MainViewCategory.Tabs:
                        this.mainViewModel.CurrentTabsDirectory = value.Directory;
                        break;
                }

                this.navigationService.GoBack();
            }
        }

        public string Title
        {
            get
            {
                switch (Category)
                {
                    case MainViewCategory.Bookmarks:
                        return string.Format("{0} - {1}", "BOOKMARKS", App.ApplicationTitle);
                    case MainViewCategory.History:
                        return string.Format("{0} - {1}", "HISTORY", App.ApplicationTitle);
                    case MainViewCategory.Tabs:
                        return string.Format("{0} - {1}", "TABS", App.ApplicationTitle);
                    default:
                        return App.ApplicationTitle;
                }
            }
        }

        private void UpdateDirectories()
        {
            Directories.Clear();

            switch (Category)
            { 
                case MainViewCategory.Bookmarks:
                    UpdateBookmarkDirectories();
                    break;
                case MainViewCategory.History:
                    UpdateHistoryDirectories();
                    break;
                case MainViewCategory.Tabs:
                    UpdateTabsDirectories();
                    break;
            }
        }

        private void UpdateBookmarkDirectories()
        {
            if (storage.Bookmarks != null)
            {
                IEnumerable<DirectoryViewModel> viewModels = ConvertToDirectoryViewModels(storage.Bookmarks.Directories);
                foreach (DirectoryViewModel viewModel in viewModels)
                    Directories.Add(viewModel);
            }
        }

        private void UpdateHistoryDirectories()
        {
            if (storage.Bookmarks != null)
            {
                // Recursively get all the directories
                IEnumerable<Directory> directories = from directory in storage.History.Directories.
                                                         Descendants(child => child.Directories)
                                                     select directory;

                foreach (Directory directory in directories)
                    Directories.Add(new DirectoryViewModel(directory));
            }
        }

        private void UpdateTabsDirectories()
        {
            if (storage.Bookmarks != null)
            {
                // Recursively get all the directories
                IEnumerable<Directory> directories = from directory in storage.Tabs.Directories.
                                                         Descendants(child => child.Directories)
                                                     select directory;

                foreach (Directory directory in directories)
                    Directories.Add(new DirectoryViewModel(directory));
            }
        }

        private IEnumerable<DirectoryViewModel> ConvertToDirectoryViewModels(IEnumerable<Directory> rootDirectories)
        {
            List<DirectoryViewModel> directoryViewModels = new List<DirectoryViewModel>();
            Thickness startMargin = new Thickness(0.0);

            foreach (Directory directory in rootDirectories)
            {
                directoryViewModels.Add(new DirectoryViewModel(directory) { Margin = startMargin });
                GetDirectoriesInternal(directory.Directories, directoryViewModels, startMargin);
            }

            return directoryViewModels;
        }

        private void GetDirectoriesInternal(IEnumerable<Directory> directories,
            IList<DirectoryViewModel> directoryViewModels, Thickness currentMargin)
        {
            Thickness newMargin = new Thickness(currentMargin.Left + 24.0, currentMargin.Top, 
                currentMargin.Right, currentMargin.Bottom);

            foreach (Directory directory in directories)
            {
                directoryViewModels.Add(new DirectoryViewModel(directory) { Margin = newMargin });
                GetDirectoriesInternal(directory.Directories, directoryViewModels, newMargin);
            }
        }
    }
}
