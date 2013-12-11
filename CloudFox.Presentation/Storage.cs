using System;
using System.Linq;
using System.Net;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Threading;
using CloudFox.Presentation.Util;
using System.Runtime.Serialization;
using System.IO;
using CloudFox.Util;

using WeaveBookmark = CloudFox.Weave.Bookmark;
using WeaveClientSession = CloudFox.Weave.ClientSession;
using System.Security.Cryptography;
using System.Text;

namespace CloudFox.Presentation
{
    public class Storage : IStorage
    {
        private bool isBookmarksLoaded;
        private bool isHistoryLoaded;
        private bool isTabsLoaded;
        private bool isProfileLoaded;

        private Directory bookmarks;
        private Directory history;
        private Directory tabs;

        private object bookmarksLock = new object();
        private object historyLock = new object();
        private object tabsLock = new object();
        private object profileLock = new object();

        public Storage()
        {
            UserName = GetSettingValue<string>("userName");
            Password = GetSettingValue<string>("password");
            Passphrase = GetSettingValue<string>("passphrase");
            UseDefaultServer = GetSettingValue<bool>("useDefaultServer", true);
            ServerAddress = GetSettingValue<string>("serverAddress");
            SynchronizeBookmarks = GetSettingValue<bool>("synchronizeBookmarks", true);
            SynchronizeHistory = GetSettingValue<bool>("synchronizeHistory");
            SynchronizeTabs = GetSettingValue<bool>("synchronizeTabs");
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Passphrase { get; set; }

        public bool UseDefaultServer { get; set; }

        public string ServerAddress { get; set; }

        public bool SynchronizeBookmarks { get; set; }

        public bool SynchronizeHistory { get; set; }

        public bool SynchronizeTabs { get; set; }

        public Directory Bookmarks
        {
            get
            {
                if (!isBookmarksLoaded)
                    LoadBookmarks();
                return bookmarks;
            }
            set
            {
                bookmarks = value;
            }
        }

        public Directory History
        {
            get
            {
                if (!isHistoryLoaded)
                    LoadHistory();
                return history;
            }
            set
            {
                history = value;
            }
        }

        public Directory Tabs
        {
            get
            {
                if (!isTabsLoaded)
                    LoadTabs();
                return tabs;
            }
            set
            {
                tabs = value;
            }
        }

        public ProfileInformation Profile { get; set; }

        public void Persist()
        {
            SetSettingValue("userName", UserName);
            SetSettingValue("password", Password);
            SetSettingValue("passphrase", Passphrase);
            SetSettingValue("useDefaultServer", UseDefaultServer);
            SetSettingValue("serverAddress", ServerAddress);
            SetSettingValue("synchronizeBookmarks", SynchronizeBookmarks);
            SetSettingValue("synchronizeHistory", SynchronizeHistory);
            SetSettingValue("synchronizeTabs", SynchronizeTabs);

            IsolatedStorageSettings.ApplicationSettings.Save();
        }

        public void PersistBookmarks()
        {
            lock (bookmarksLock)    // We want to be sure that only one invocation at a time writes to the files.
            {
                PersistToFile<Directory>("bookmarks.xml", Bookmarks);
            }
        }

        public void PersistHistory()
        {
            lock (historyLock)  // We want to be sure that only one invocation at a time writes to the files.
            {
                PersistToFile<Directory>("history.xml", History);
            }
        }

        public void PersistTabs()
        {
            lock (tabsLock) // We want to be sure that only one invocation at a time writes to the files.
            {
                PersistToFile<Directory>("tabs.xml", Tabs);
            }
        }

        public void PersistProfile()
        {
            lock (profileLock)  // We want to be sure that only one invocation at a time writes to the files.
            {
                PersistToFile<ProfileInformation>("profile.xml", Profile);
            }
        }

        public IAsyncResult BeginLoadBookmarks(AsyncCallback callback, object state)
        {
            AsyncResult asyncResult = new AsyncResult(callback, state);

            if (!isBookmarksLoaded && !string.IsNullOrEmpty(UserName)) 
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        LoadBookmarks();
                        asyncResult.SetAsCompleted(null, false);
                    }
                    catch (Exception ex)
                    {
                        asyncResult.SetAsCompleted(ex, false);
                    }
                });
            }
            else
            {
                // If Bookmarks != null, then the bookmarks are already loaded
                // or if UserName == null then no bookmarks can be loaded
                // So we don't use the threadpool and directly stop loading for performance reasons
                asyncResult.SetAsCompleted(null, true);
            }

            return asyncResult;
        }

        public void EndLoadBookmarks(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            AsyncResult internalAsyncResult = asyncResult as AsyncResult;

            if (internalAsyncResult == null)
                throw new ArgumentException("Invalid IAsyncResult instance.", "asyncResult");

            internalAsyncResult.EndInvoke();
        }

        public void LoadBookmarks()
        {
            if (!isBookmarksLoaded)
            {
                Bookmarks = LoadFromFile<Directory>("bookmarks.xml");
                isBookmarksLoaded = true;
            }
        }

        public void LoadHistory()
        {
            if (!isHistoryLoaded)
            {
                History = LoadFromFile<Directory>("history.xml");
                isHistoryLoaded = true;
            }
        }

        public void LoadTabs()
        {
            if (!isTabsLoaded)
            {
                Tabs = LoadFromFile<Directory>("tabs.xml");
                isTabsLoaded = true;
            }
        }

        public IAsyncResult BeginLoadProfile(AsyncCallback callback, object state)
        {
            AsyncResult asyncResult = new AsyncResult(callback, state);

            if (!isProfileLoaded && !string.IsNullOrEmpty(UserName))
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    try
                    {
                        LoadProfile();
                        asyncResult.SetAsCompleted(null, false);
                    }
                    catch (Exception ex)
                    {
                        asyncResult.SetAsCompleted(ex, false);
                    }
                });
            }
            else
            {
                // If Profile != null, then the bookmarks are already loaded
                // or if UserName == null then no bookmarks can be loaded
                // So we don't use the threadpool and directly stop loading for performance reasons
                asyncResult.SetAsCompleted(null, true);
            }

            return asyncResult;
        }

        public void EndLoadProfile(IAsyncResult asyncResult)
        {
            if (asyncResult == null)
                throw new ArgumentNullException("asyncResult");

            AsyncResult internalAsyncResult = asyncResult as AsyncResult;

            if (internalAsyncResult == null)
                throw new ArgumentException("Invalid IAsyncResult instance.", "asyncResult");

            internalAsyncResult.EndInvoke();
        }

        public void LoadProfile()
        {
            if (!isProfileLoaded)
            {
                Profile = LoadFromFile<ProfileInformation>("profile.xml");
                isProfileLoaded = true;
            }
        }

        public IAsyncResult BeginSearch(string searchText, AsyncCallback callback, object state)
        {
            return AsyncHelper.BeginInvoke(() => Search(searchText), callback, state);
        }

        public IEnumerable<Bookmark> EndSearch(IAsyncResult asyncResult)
        {
            return AsyncHelper.EndInvoke<IEnumerable<Bookmark>>(asyncResult);
        }

        public IEnumerable<Bookmark> Search(string searchText)
        {
            IEnumerable<Bookmark> searchResults = new Bookmark[0];

            if (Bookmarks != null)
            {
                searchResults =
                    searchResults.Concat(from bookmark in
                                             (from directory in Bookmarks.Directories.Descendants(child => child.Directories)
                                              select directory.Bookmarks).Flatten()
                                         where IsSearchMatch(bookmark, searchText)
                                         select bookmark);
            }

            if (History != null)
            {
                searchResults =
                    searchResults.Concat(from bookmark in
                                             (from directory in History.Directories.Descendants(child => child.Directories)
                                              select directory.Bookmarks).Flatten()
                                         where IsSearchMatch(bookmark, searchText)
                                         select bookmark);
            }

            if (Tabs != null)
            {
                searchResults =
                    searchResults.Concat(from bookmark in
                                             (from directory in Tabs.Directories.Descendants(child => child.Directories)
                                              select directory.Bookmarks).Flatten()
                                         where IsSearchMatch(bookmark, searchText)
                                         select bookmark);
            }

            return searchResults;
        }

        /// <summary>
        /// Persist a data structure to a file in isolated storage for the current user account.
        /// </summary>
        /// <typeparam name="T">The data type to persist</typeparam>
        /// <param name="fileName">The name of the file in isolated storage.</param>
        /// <param name="data">The data structure to persist.</param>
        private void PersistToFile<T>(string fileName, T data)
        {
            string filePath = GetAccountFilePath(fileName);

            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication();
            userStore.CreateDirectory(Path.GetDirectoryName(filePath));

            using (Stream stream = userStore.CreateFile(filePath))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(stream, data);
            }
        }

        /// <summary>
        /// Load a data structure from a file in isolated storage for the current user account.
        /// </summary>
        /// <typeparam name="T">The data type to load</typeparam>
        /// <param name="fileName">The name of the file in isolated storage.</param>
        /// <returns>The loaded data structure.</returns>
        private T LoadFromFile<T>(string fileName)
        {
            string filePath = GetAccountFilePath(fileName);

            IsolatedStorageFile userStore = IsolatedStorageFile.GetUserStoreForApplication();

            if (userStore.FileExists(filePath))
            {
                using (Stream stream = userStore.OpenFile(filePath, FileMode.Open))
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    return (T)serializer.ReadObject(stream);
                }
            }
            else
                return default(T);
        }

        /// <summary>
        /// Get the isolated storage file path for the account used.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <returns>An isolated storage file path for the given file.</returns>
        private string GetAccountFilePath(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException("fileName");

            string path;

            if(UseDefaultServer)
                path = Path.Combine("accounts", UserName + "@" + "default");
            else
                path = Path.Combine("accounts", UserName + "@" + ServerAddress);

            return Path.Combine(path, fileName);
        }

        /// <summary>
        /// Get the value of a setting from isolated storage.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="name">The name of the setting.</param>
        /// <returns>The value of the setting or the default value if it could not be found in isolated storage.</returns>
        private static T GetSettingValue<T>(string name)
        {
            return GetSettingValue<T>(name, default(T));
        }

        /// <summary>
        /// Get the value of a setting from isolated storage, or the provided default value if 
        /// it could not be found in isolated storage.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="name">The name of the setting.</param>
        /// <param name="defaultValue">The default value if the setting could not be found in isolated storage.</param>
        /// <returns>The value of the setting or provided the default value if it could not be found in isolated storage.</returns>
        private static T GetSettingValue<T>(string name, T defaultValue)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains(name))
            {
                object storedValue = IsolatedStorageSettings.ApplicationSettings[name];
                if (storedValue != null && IsEncryptedValue(typeof(T), storedValue.GetType()))
                    return DecryptValue<T>(storedValue);
                else
                    return (T)storedValue;
            }
            else
                return defaultValue;
        }

        /// <summary>
        /// Store the value of a setting in isolated storage.
        /// </summary>
        /// <typeparam name="T">The type of the setting.</typeparam>
        /// <param name="key">The name of the setting.</param>
        /// <param name="value">The value of the setting.</param>
        private static void SetSettingValue<T>(string key, T value)
        {
            object valueToStore;
            if (value is string)
                valueToStore = ProtectedData.Protect(Encoding.UTF8.GetBytes(value as string), null);
            else
                valueToStore = value;

            if (IsolatedStorageSettings.ApplicationSettings.Contains(key))
                IsolatedStorageSettings.ApplicationSettings[key] = valueToStore;
            else
                IsolatedStorageSettings.ApplicationSettings.Add(key, valueToStore);
        }

        /// <summary>
        /// Determine if the value was encrypted or not. If the data type is byte[]
        /// then no encryption is assumed.
        /// </summary>
        /// <param name="expectedType">The expected type of the value.</param>
        /// <param name="actualType">The actual type of the value.</param>
        /// <returns>Returns true if the value was encrypted.</returns>
        private static bool IsEncryptedValue(Type expectedType, Type actualType)
        {
            return expectedType != actualType && actualType == typeof(byte[]);
        }

        /// <summary>
        /// Decrypt the value specified as input to an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The expected output type.</typeparam>
        /// <param name="value">The value that must be decrypted to the specified type.</param>
        /// <returns>The decrypted data in the type specified.</returns>
        private static T DecryptValue<T>(object value)
        {
            byte[] encryptedData = value as byte[];

            if (encryptedData == null)
                throw new ArgumentException("Only a valid byte array is accepted as input.", "value");

            Type targetType = typeof(T);
            if (targetType == typeof(string))
            {
                byte[] decryptedData = ProtectedData.Unprotect(encryptedData, null);
                return (T)(object)Encoding.UTF8.GetString(decryptedData, 0, decryptedData.Length);
            }
            else
            {
                throw new InvalidOperationException("Currently only the encryption and decryption of string value is supported.");
            }
        }

        /// <summary>
        /// Determine if the bookmark maches with the provided search text.
        /// </summary>
        /// <param name="bookmark">The bookmark that must be be matched with the search text.</param>
        /// <param name="searchText">The search text entered by the user.</param>
        /// <returns>Returns true if the bookmark matches with the provided search text, false otherwise.</returns>
        private static bool IsSearchMatch(Bookmark bookmark, string searchText)
        {
            return (bookmark.Title != null && bookmark.Title.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) > -1) ||
                (bookmark.Location != null && bookmark.Location.IndexOf(searchText, StringComparison.InvariantCultureIgnoreCase) > -1);
        }
    }
}
