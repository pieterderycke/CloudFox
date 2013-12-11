using System;
using System.Net;
using System.Linq;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using CloudFox.Presentation.Util;
using System.Collections.ObjectModel;
using System.Threading;
using System.Windows.Threading;
using System.Windows;
using System.ComponentModel;
using Microsoft.Phone.Reactive;
using Microsoft.Phone.Tasks;

namespace CloudFox.Presentation.ViewModels
{
    public class SearchViewModel : ViewModelBase
    {
        private IStorage storage;

        private string searchText;

        public SearchViewModel(IStorage storage)
        {
            this.storage = storage;

            SearchResults = new ObservableCollection<BookmarkViewModel>();
        }

        public string SearchText
        {
            get
            {
                return this.searchText;
            }
            set
            {
                if (value != this.searchText)
                {
                    this.searchText = value;
                    SearchResults.Clear();

                    IObservable<IEnumerable<Bookmark>> observable = Observable.FromAsyncPattern<IEnumerable<Bookmark>>(
                        (callback, state) => storage.BeginSearch(searchText, callback, state),
                        storage.EndSearch)();
                    observable.ObserveOn(Deployment.Current.Dispatcher).Subscribe(result => 
                        {
                            foreach (Bookmark bookmark in result)
                            {
                                SearchResults.Add(new BookmarkViewModel(bookmark));
                            }
                        });
                }
            }
        }

        public ObservableCollection<BookmarkViewModel> SearchResults { get; private set; }

        public BookmarkViewModel SelectedSearchResult
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
    }
}
