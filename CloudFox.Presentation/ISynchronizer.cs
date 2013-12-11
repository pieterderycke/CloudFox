using System;
using System.ComponentModel;

namespace CloudFox.Presentation
{
    public delegate void ProgressChangedEventHandler(RefreshWeaveDataProgressChangedEventArgs e);

    public delegate void RefreshWeaveDataCompletedEventHandler(object sender, RefreshWeaveDataCompletedEventArgs e);

    public interface ISynchronizer
    {
        void RefreshWeaveDataAsync(object userState);

        event RefreshWeaveDataCompletedEventHandler RefreshWeaveDataCompleted;
        event ProgressChangedEventHandler RefreshWeaveDataProgressChanged;
    }
}
