using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using CloudFox.Weave;

using WeaveBookmark = CloudFox.Weave.Bookmark;

namespace CloudFox.Presentation
{
    public static class BookmarksStructureBuilder
    {
        /// <summary>
        /// Build a hierarchic directory structure.
        /// </summary>
        /// <param name="bookmarks"></param>
        /// <returns></returns>
        public static Directory Build(IEnumerable<WeaveBookmark> bookmarks)
        {
            Dictionary<string, Directory> directoryIdMapping = new Dictionary<string, Directory>();
            Dictionary<string, Bookmark> bookmarkIdMapping = new Dictionary<string, Bookmark>();

            IList<WeaveBookmark> directories = (from b in bookmarks
                                                where b.BookmarkType == BookmarkType.Folder
                                                select b).ToList();

            IList<WeaveBookmark> items = (from b in bookmarks
                                                where b.BookmarkType == BookmarkType.Bookmark
                                                select b).ToList();

            // Create directory objects
            foreach (WeaveBookmark weaveDirectory in directories)
            {
                directoryIdMapping.Add(weaveDirectory.Id, new Directory(weaveDirectory.Title, weaveDirectory.Id));
            }

            // Create bookmark objects
            foreach (WeaveBookmark weaveBookmark in items)
            {
                bookmarkIdMapping.Add(weaveBookmark.Id, new Bookmark(weaveBookmark.Title, weaveBookmark.Uri));
            }

            // Assign parent values
            foreach (WeaveBookmark weaveDirectory in directories)
            {
                if (directoryIdMapping.ContainsKey(weaveDirectory.ParentId))
                    directoryIdMapping[weaveDirectory.Id].Parent = directoryIdMapping[weaveDirectory.ParentId];
            }

            // Assign items
            foreach (WeaveBookmark weaveDirectory in directories)
            {
                Directory directory = directoryIdMapping[weaveDirectory.Id];

                foreach (string child in weaveDirectory.Children)
                {
                    if (bookmarkIdMapping.ContainsKey(child))
                    {
                        Bookmark bookmark = bookmarkIdMapping[child];
                        directory.Bookmarks.Add(bookmark);
                    }
                    else if (directoryIdMapping.ContainsKey(child))
                    {
                        Directory childDirectory = directoryIdMapping[child];
                        directory.Directories.Add(childDirectory);
                    }
                }
            }

            Directory bookmarksDirectory = new Directory("All Bookmarks", "bookmarks");
            bookmarksDirectory.Directories.Add(directoryIdMapping["toolbar"]);
            bookmarksDirectory.Directories.Add(directoryIdMapping["menu"]);
            return bookmarksDirectory;
        }
    }
}
