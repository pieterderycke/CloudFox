using System;
using System.Linq;
using System.Net;
using CloudFox.Presentation.Util;
using System.ComponentModel;
using System.Threading;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using CloudFox.Weave;

using WeaveBookmark = CloudFox.Weave.Bookmark;
using WeaveClient = CloudFox.Weave.Client;
using WeaveClientSession = CloudFox.Weave.ClientSession;
using WeaveTab = CloudFox.Weave.Tab;
using WeaveHistoryItem = CloudFox.Weave.HistoryItem;
using System.Windows;
using CloudFox.Presentation.Localization;

namespace CloudFox.Presentation
{
    public class Synchronizer : ISynchronizer
    {
        private Dictionary<object, AsyncOperation> userStateToLifetime;

        private IStorage storage;

        public Synchronizer(IStorage storage)
        {
            this.storage = storage;
            this.userStateToLifetime = new Dictionary<object, AsyncOperation>();
        }

        public void RefreshWeaveDataAsync(object userState)
        {
            AsyncOperation asyncOperation = AsyncOperationManager.CreateOperation(userState);

            // Multiple threads will access the user state dictionary,
            // so it must be locked to serialize access.
            lock (this)
            {
                if (userStateToLifetime.ContainsKey(userState))
                {
                    throw new ArgumentException("User State parameter must be unique.", "userState");
                }

                userStateToLifetime.Add(userState, asyncOperation);
            }

            ThreadPool.QueueUserWorkItem(o =>
                {
                    RefreshWeaveDataWorker(asyncOperation);
                });
        }

        public event RefreshWeaveDataCompletedEventHandler RefreshWeaveDataCompleted;
        public event ProgressChangedEventHandler RefreshWeaveDataProgressChanged;

        /// <summary>
        /// Method performing the actual work of the operation RefreshWeaveDataAsync.
        /// </summary>
        /// <param name="asyncOperation">An <see cref="AsyncOperation"/> object.</param>
        private void RefreshWeaveDataWorker(AsyncOperation asyncOperation)
        {
            try
            {
                if (string.IsNullOrEmpty(storage.UserName))
                {
                    throw new Exception(AppResources.InvalidUserName);
                }

                if (string.IsNullOrEmpty(storage.Password))
                {
                    throw new Exception(AppResources.InvalidPassword);
                }

                if (string.IsNullOrEmpty(storage.Passphrase))
                {
                    throw new Exception(AppResources.InvalidSyncKey);
                }

                if (!storage.UseDefaultServer && string.IsNullOrEmpty(storage.ServerAddress))
                {
                    throw new Exception(AppResources.InvalidCustomServerAddress);
                }

                if (!NetworkInterface.GetIsNetworkAvailable())
                {
                    throw new Exception(AppResources.NoNetwork);
                }

                RefreshWeaveDataProgressUpdateMethod(AppResources.ConnectingToWeaveServer, asyncOperation);

                WeaveProxy proxy;

                if (storage.UseDefaultServer)
                    proxy = new WeaveProxy(new HttpCommunicationChannel(), storage.UserName, storage.Password, storage.Passphrase);
                else
                    proxy = new WeaveProxy(new HttpCommunicationChannel(), storage.UserName, storage.Password, storage.Passphrase, storage.ServerAddress);

                RefreshWeaveDataProgressUpdateMethod(AppResources.UpdatingProfile, asyncOperation);

                // Download collection information
                Dictionary<string, int> collectionCounts = proxy.GetCollectionCounts();
                Dictionary<string, int> collectionUsage = proxy.GetCollectionUsage();

                // Download clients
                IEnumerable<WeaveBasicObject> encryptedClients = proxy.GetCollection("clients", SortOrder.Index);
                IEnumerable<WeaveClient> weaveClients = proxy.DecryptPayload<WeaveClient>(encryptedClients);

                // Update the storage
                this.storage.Profile = new ProfileInformation(proxy.WeaveNode, proxy.ServerStorageVersion,
                    CombineCollectionInformation(collectionCounts, collectionUsage), ConvertWeaveClients(weaveClients));
                this.storage.PersistProfile();

                if (this.storage.SynchronizeBookmarks)
                {
                    RefreshWeaveDataProgressUpdateMethod(AppResources.UpdatingBookmarks, asyncOperation);

                    // Download bookmarks
                    IEnumerable<WeaveBasicObject> encryptedWeaveBookmarks = proxy.GetCollection("bookmarks", SortOrder.Index);
                    if (encryptedWeaveBookmarks != null)
                    {
                        IEnumerable<WeaveBookmark> weaveBookmarks = proxy.DecryptPayload<WeaveBookmark>(encryptedWeaveBookmarks);

                        Directory rootBookmarksDirectory = BookmarksStructureBuilder.Build(weaveBookmarks);

                        // Update the storage
                        this.storage.Bookmarks = rootBookmarksDirectory;
                        this.storage.PersistBookmarks();
                    }
                }

                if (this.storage.SynchronizeHistory)
                {
                    RefreshWeaveDataProgressUpdateMethod(AppResources.UpdatingHistory, asyncOperation);

                    // Download history
                    IEnumerable<WeaveBasicObject> encryptedWeaveHistory = proxy.GetCollection("history", SortOrder.Index);
                    if (encryptedWeaveHistory != null)
                    {
                        IEnumerable<WeaveHistoryItem> weaveHistory = proxy.DecryptPayload<WeaveHistoryItem>(encryptedWeaveHistory).ToList();

                        Directory rootHistoryDirectory = HistoryStructureBuilder.Build(weaveHistory);
                        this.storage.PersistBookmarks();

                        // Update the storage
                        this.storage.History = rootHistoryDirectory;
                        this.storage.PersistHistory();
                    }
                }

                if (this.storage.SynchronizeTabs)
                {
                    RefreshWeaveDataProgressUpdateMethod(AppResources.UpdatingTabs, asyncOperation);

                    // Download opened tabs
                    IEnumerable<WeaveBasicObject> encryptedWeaveTabs = proxy.GetCollection("tabs", SortOrder.Index);
                    if (encryptedWeaveTabs != null)
                    {
                        IEnumerable<WeaveClientSession> weaveClientSessions = proxy.DecryptPayload<WeaveClientSession>(encryptedWeaveTabs);

                        // Update the storage
                        this.storage.Tabs = OpenTabsStructureBuilder.Build(weaveClientSessions);
                        this.storage.PersistTabs();
                    }
                }

                RefreshWeaveDataCompletionMethod(null, false, asyncOperation);
            }
            catch (Exception ex)
            { 
                RefreshWeaveDataCompletionMethod(ex, false, asyncOperation);
            }
        }

        // This is the method that the underlying, free-threaded 
        // asynchronous behavior will invoke.  This will happen on
        // an arbitrary thread.
        private void RefreshWeaveDataCompletionMethod(Exception error, bool cancelled, AsyncOperation asyncOperation)
        {
            // If the task was not previously canceled,
            // remove the task from the lifetime collection.
            if (!cancelled)
            {
                lock (this)
                {
                    userStateToLifetime.Remove(asyncOperation.UserSuppliedState);
                }
            }

            RefreshWeaveDataCompletedEventArgs e = new RefreshWeaveDataCompletedEventArgs(error, cancelled, asyncOperation.UserSuppliedState);
            asyncOperation.PostOperationCompleted(state => OnRefreshWeaveDataCompleted((RefreshWeaveDataCompletedEventArgs)state), e);
        }

        private void RefreshWeaveDataProgressUpdateMethod(string message, AsyncOperation asyncOperation)
        {
            RefreshWeaveDataProgressChangedEventArgs e = new RefreshWeaveDataProgressChangedEventArgs(message, asyncOperation.UserSuppliedState);
            asyncOperation.Post(state => OnProgressChanged((RefreshWeaveDataProgressChangedEventArgs)state), e);
        }

        private void OnRefreshWeaveDataCompleted(RefreshWeaveDataCompletedEventArgs e)
        {
            if (RefreshWeaveDataCompleted != null)
            {
                RefreshWeaveDataCompleted(this, e);
            }
        }

        private void OnProgressChanged(RefreshWeaveDataProgressChangedEventArgs e)
        {
            if (RefreshWeaveDataProgressChanged != null)
            {
                RefreshWeaveDataProgressChanged(e);
            }
        }

        /// <summary>
        /// Combine the item counts for each collection and the usage in Kb for each collection.
        /// </summary>
        /// <param name="collectionCounts">A dictionary containg the item count for each collection.</param>
        /// <param name="collectionUsage">A dictionary containg the usage for each collection.</param>
        /// <returns>An collection containing all the collection information.</returns>
        private static IList<CollectionInformation> CombineCollectionInformation(Dictionary<string, int> collectionCounts,
            Dictionary<string, int> collectionUsage)
        {
            List<CollectionInformation> collections = new List<CollectionInformation>();

            foreach (string collectionName in collectionCounts.Keys)
            {
                int itemCount = collectionCounts[collectionName];
                int size = collectionUsage[collectionName];

                collections.Add(new CollectionInformation(collectionName, itemCount, size));
            }

            return collections;
        }

        private static IEnumerable<Client> ConvertWeaveClients(IEnumerable<WeaveClient> weaveClients)
        {
            foreach (WeaveClient weaveClient in weaveClients)
                yield return new Client(weaveClient.Id, weaveClient.Name, weaveClient.Type);
        }
    }
}
