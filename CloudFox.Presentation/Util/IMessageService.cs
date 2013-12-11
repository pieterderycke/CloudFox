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

namespace CloudFox.Presentation.Util
{
    public interface IMessageService
    {
        void ShowErrorMessage(string message);
        void ShowInformationMessage(string message);
        void ShowToastMessage(string message);
        void ShowProgressMessage(string message, bool indeterminate);
        void StopProgressMessage();
        void StopProgressMessage(int delay);
    }
}
