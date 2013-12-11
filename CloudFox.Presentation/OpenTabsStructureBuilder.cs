using System;
using System.Collections.Generic;

using WeaveClientSession = CloudFox.Weave.ClientSession;
using WeaveTab = CloudFox.Weave.Tab;

namespace CloudFox.Presentation
{
    public class OpenTabsStructureBuilder
    {
        public static Directory Build(IEnumerable<WeaveClientSession> clientSessions)
        {
            Directory rootDirectory = new Directory("Open Tabs", "tabs");
            foreach (Directory directory in ConvertWeaveClientSessions(clientSessions))
                rootDirectory.Directories.Add(directory);

            return rootDirectory;
        }

        private static IList<Directory> ConvertWeaveClientSessions(IEnumerable<WeaveClientSession> weaveClientSessions)
        {
            List<Directory> directories = new List<Directory>();

            foreach (WeaveClientSession weaveClientSession in weaveClientSessions)
            {
                Directory clientDirectory = new Directory(weaveClientSession.ClientName, weaveClientSession.ClientName);
                directories.Add(clientDirectory);

                foreach (WeaveTab weaveTab in weaveClientSession.OpenTabs)
                    clientDirectory.Bookmarks.Add(new Bookmark(weaveTab.Title, weaveTab.UrlHistory[0]));
            }

            return directories;
        }
    }
}
