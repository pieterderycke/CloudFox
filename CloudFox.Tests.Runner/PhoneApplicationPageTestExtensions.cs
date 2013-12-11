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
using Microsoft.Phone.Shell;
using Microsoft.Silverlight.Testing;
using Microsoft.Phone.Controls;
using System.Reflection;
using System.Collections.Generic;

namespace CloudFox.Tests.Runner
{
    public static class PhoneApplicationPageTestExtensions
    {
        public static void StartTestRunner(this PhoneApplicationPage mainPage, IEnumerable<Assembly> testAssemblies)
        {
            SystemTray.IsVisible = false;

            UnitTestSettings testSettings = UnitTestSystem.CreateDefaultSettings();
            foreach (Assembly assembly in testAssemblies)
                testSettings.TestAssemblies.Add(assembly);

            var testPage = UnitTestSystem.CreateTestPage(testSettings) as IMobileTestPage;
            mainPage.BackKeyPress += (x, xe) => xe.Cancel = testPage.NavigateBack();
            (Application.Current.RootVisual as PhoneApplicationFrame).Content = testPage;
        }
    }
}
