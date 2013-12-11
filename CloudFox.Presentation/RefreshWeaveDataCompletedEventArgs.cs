using System;
using System.ComponentModel;

namespace CloudFox.Presentation
{
    public class RefreshWeaveDataCompletedEventArgs : AsyncCompletedEventArgs
    {
        public RefreshWeaveDataCompletedEventArgs(Exception error, bool cancelled, object userState)
            : base(error, cancelled, userState)
        {
        }
    }
}
