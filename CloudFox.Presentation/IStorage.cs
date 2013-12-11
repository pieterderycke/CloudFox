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
using System.Collections.Generic;

namespace CloudFox.Presentation
{
    public interface IStorage
    {
        string UserName { get; set; }
        string Password { get; set; }
        string Passphrase { get; set; }
        bool UseDefaultServer { get; set; }
        string ServerAddress { get; set; }
        bool SynchronizeBookmarks { get; set; }
        bool SynchronizeHistory { get; set; }
        bool SynchronizeTabs { get; set; }
        Directory Bookmarks { get; set; }
        Directory History { get; set; }
        Directory Tabs { get; set; }
        ProfileInformation Profile { get; set; }

        /// <summary>
        /// Persist the regular application parameters to durable storage.
        /// </summary>
        void Persist();

        /// <summary>
        /// Persist the bookmarks to durable storage.
        /// </summary>
        void PersistBookmarks();

        /// <summary>
        /// Persist the history to durable storage.
        /// </summary>
        void PersistHistory();

        /// <summary>
        /// Persist the tabs to durable storage.
        /// </summary>
        void PersistTabs();

        /// <summary>
        /// Persist the profile information to durable storage.
        /// </summary>
        void PersistProfile();

        /// <summary>
        /// Beging loading the bookmarks from durable storage asynchronously.
        /// </summary>
        IAsyncResult BeginLoadBookmarks(AsyncCallback callback, object state);

        /// <summary>
        /// End loading the bookmarks from durable storage asynchronously.
        /// </summary>
        void EndLoadBookmarks(IAsyncResult asyncResult);

        /// <summary>
        /// Load the bookmarks from durable storage.
        /// </summary>
        void LoadBookmarks();

        /// <summary>
        /// Load the history from durable storage.
        /// </summary>
        void LoadHistory();

        /// <summary>
        /// Load the tabs from durable storage.
        /// </summary>
        void LoadTabs();

        /// <summary>
        /// Beging loading the profile information from durable storage asynchronously.
        /// </summary>
        IAsyncResult BeginLoadProfile(AsyncCallback callback, object state);

        /// <summary>
        /// End loading the profile from durable storage asynchronously.
        /// </summary>
        void EndLoadProfile(IAsyncResult asyncResult);

        /// <summary>
        /// Load the profile from durable storage.
        /// </summary>
        void LoadProfile();

        /// <summary>
        /// Begin searching in the bookmarks, history and open tabs contained in this storage asynchronously.
        /// </summary>
        IAsyncResult BeginSearch(string searchText, AsyncCallback callback, object state);

        /// <summary>
        /// End searching in the bookmarks, history and open tabs contained in this storage asynchronously.
        /// </summary>
        IEnumerable<Bookmark> EndSearch(IAsyncResult asyncResult);

        /// <summary>
        /// Search in the bookmarks, history and open tabs contained in this storage
        /// </summary>
        IEnumerable<Bookmark> Search(string searchText);
    }
}
