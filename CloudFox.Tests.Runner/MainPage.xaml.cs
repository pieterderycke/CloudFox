using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Reflection;
using CloudFox.Weave.Tests;

namespace CloudFox.Tests.Runner
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            this.StartTestRunner(FindTestAssemblies());
        }

        private static IEnumerable<Assembly> FindTestAssemblies()
        {
            yield return typeof(WeaveKeysTests).Assembly;
        }
    }
}