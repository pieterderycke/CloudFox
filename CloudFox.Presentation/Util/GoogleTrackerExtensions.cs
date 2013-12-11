using GoogleAnalytics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudFox.Presentation.Util
{
    public static class GoogleTrackerExtensions
    {
        public static void SendCurrentView(this Tracker tracker)
        {
            StackTrace stackTrace = new StackTrace();
            Type callerType = stackTrace.GetFrame(1).GetMethod().DeclaringType;
            string name = callerType.Name;

            GoogleAnalytics.EasyTracker.GetTracker().SendView(name);
        }
    }
}
