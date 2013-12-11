using System;
using System.ComponentModel;

namespace CloudFox.Presentation
{
    public class RefreshWeaveDataProgressChangedEventArgs : ProgressChangedEventArgs
    {
        public RefreshWeaveDataProgressChangedEventArgs(string message, object userState)
            : base(0, userState)
        {
            this.Message = message;
        }

        public string Message { get; private set; }
    }
}
