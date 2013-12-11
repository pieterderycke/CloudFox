using System;
using System.Linq;
using System.Collections.Generic;
using CloudFox.Presentation.Util;

using WeaveHistoryItem = CloudFox.Weave.HistoryItem;

namespace CloudFox.Presentation
{
    public class HistoryStructureBuilder
    {
        public static Directory Build(IEnumerable<WeaveHistoryItem> weaveHistoryItems)
        {
            if (weaveHistoryItems.IsEmpty())
            {
                return new Directory("All History", "history");
            }
            else
            {
                // Today
                var todayQuery = from b in weaveHistoryItems
                                 let dateTime = b.Visits!= null ? b.Visits.First().Date : DateTime.MinValue
                                 where IsToday(dateTime)
                                 orderby dateTime
                                 select b;

                Directory today = new Directory("Today", "today");
                foreach (WeaveHistoryItem historyItem in todayQuery)
                    today.Bookmarks.Add(new Bookmark(historyItem.Title, historyItem.Uri));

                // Yesterday
                var yesterdayQuery = from b in weaveHistoryItems
                                     let dateTime = b.Visits != null ? b.Visits.First().Date : DateTime.MinValue
                                     where IsYesterday(dateTime)
                                     orderby dateTime
                                     select b;

                Directory yesterday = new Directory("Yesterday", "yesterday");
                foreach (WeaveHistoryItem historyItem in yesterdayQuery)
                    yesterday.Bookmarks.Add(new Bookmark(historyItem.Title, historyItem.Uri));

                // This Month
                var thisMonthQuery = from b in weaveHistoryItems
                                     let dateTime = b.Visits != null ? b.Visits.First().Date : DateTime.MinValue
                                     where IsThisMonth(dateTime)
                                     orderby dateTime
                                     select b;

                Directory thisMonth = new Directory("This Month", "thismonth");
                foreach (WeaveHistoryItem historyItem in thisMonthQuery)
                    thisMonth.Bookmarks.Add(new Bookmark(historyItem.Title, historyItem.Uri));

                // History
                Directory history = new Directory("All History", "history");
                history.Directories.Add(today);
                history.Directories.Add(yesterday);
                history.Directories.Add(thisMonth);

                return history;
            }
        }

        private static bool IsToday(DateTime dateTime)
        {
            DateTime today = Today;

            return dateTime.Year == today.Year &&
                dateTime.Month == today.Month &&
                dateTime.Day == today.Day;
        }

        private static bool IsYesterday(DateTime dateTime)
        {
            DateTime yesterDay = YesterDay;

            return dateTime.Year == yesterDay.Year &&
                dateTime.Month == yesterDay.Month &&
                dateTime.Day == yesterDay.Day;
        }

        private static bool IsThisMonth(DateTime dateTime)
        {
            DateTime today = Today;
            DateTime yesterDay = YesterDay;

            return dateTime.Year == today.Year &&
                dateTime.Month == today.Month &&
                dateTime.Day != today.Day && dateTime.Day != yesterDay.Day;
        }

        public static DateTime Today
        {
            get
            { 
                DateTime now = DateTime.Now;
                return new DateTime(now.Year, now.Month, now.Day);
            }
        }

        public static DateTime YesterDay
        {
            get
            {
                DateTime today = Today;
                return today.AddDays(-1.0);
            }
        }
    }
}
