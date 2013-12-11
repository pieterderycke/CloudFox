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
using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Shell;
using System.Threading;

namespace CloudFox.Presentation.Util
{
    public class MessageService : IMessageService
    {
        public void ShowErrorMessage(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(message, "Error", MessageBoxButton.OK);
            });
        }

        public void ShowInformationMessage(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                MessageBox.Show(message, "Info", MessageBoxButton.OK);
            });
        }

        public void ShowToastMessage(string message)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                ToastPrompt toast = new ToastPrompt();
                toast.Title = message;
                toast.Show();
            });
        }

        public void ShowProgressMessage(string message, bool indeterminate)
        {
            if (SystemTray.ProgressIndicator != null)
            {
                ProgressIndicator progressIndicator = SystemTray.ProgressIndicator;
                progressIndicator.Text = message;

                if(progressIndicator.IsIndeterminate != indeterminate)
                    progressIndicator.IsIndeterminate = indeterminate;
            }
            else
            {
                ProgressIndicator progressIndicator = new ProgressIndicator();
                progressIndicator.IsVisible = true;
                progressIndicator.IsIndeterminate = indeterminate;
                progressIndicator.Text = message;
                SystemTray.ProgressIndicator = progressIndicator;
            }
        }

        public void StopProgressMessage()
        {
            StopProgressMessage(0);
        }

        public void StopProgressMessage(int delay)
        {
            ThreadPool.QueueUserWorkItem(state =>
                {
                    Thread.Sleep(delay);
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            SystemTray.ProgressIndicator = null;
                        });
                });
        }
    }
}
