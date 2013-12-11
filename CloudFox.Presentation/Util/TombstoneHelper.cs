using System;
using System.Linq;
using Microsoft.Phone.Controls;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Windows.Input;

namespace CloudFox.Presentation.Util
{
    /// <summary>
    /// This class contain static helper methods to ease the tombstoning of Silverlight controls.
    /// </summary>
    public static class TombstoneHelper
    {
        public static void SaveState(PhoneApplicationPage page)
        {
            foreach (PropertyInfo tombstoneProperty in FindTombstoneProperties(page.DataContext))
            {
                page.State["ViewModel." + tombstoneProperty.Name] = tombstoneProperty.GetValue(page.DataContext, null);
            }

            // We can only remember the focused control, if it has a name
            Control focusedControl = FocusManager.GetFocusedElement() as Control;
            if (focusedControl != null && focusedControl.Name != null)
            {
                page.State["FocusedControl.Name"] = focusedControl.Name;
            }
        }

        public static void SaveState(PhoneApplicationPage page, ListBox listBox)
        {
            VerifyControlHasName(listBox, "listBox");

            try
            {
                ScrollViewer viewer = (ScrollViewer)(VisualTreeHelper.GetChild(listBox, 0) as FrameworkElement).FindName("ScrollViewer");
                page.State[listBox.Name + ".VerticalOffset"] = viewer.VerticalOffset;
            }
            catch (ArgumentOutOfRangeException)
            {
                page.State[listBox.Name + ".VerticalOffset"] = 0.0;
            }
        }

        public static void SaveState(PhoneApplicationPage page, CheckBox checkBox)
        {
            VerifyControlHasName(checkBox, "checkBox");

            page.State[checkBox.Name + ".IsChecked"] = checkBox.IsChecked;
        }

        public static void SaveState(PhoneApplicationPage page, ToggleSwitch toggleSwitch)
        {
            VerifyControlHasName(toggleSwitch, "toggleSwitch");

            page.State[toggleSwitch.Name + ".IsChecked"] = toggleSwitch.IsChecked;
        }

        public static void SaveState(PhoneApplicationPage page, TextBox textBox)
        {
            VerifyControlHasName(textBox, "textBox");

            page.State[textBox.Name + ".Text"] = textBox.Text;
            page.State[textBox.Name + ".SelectionStart"] = textBox.SelectionStart;
            page.State[textBox.Name + ".SelectionLength"] = textBox.SelectionLength;
        }

        public static void SaveState(PhoneApplicationPage page, PasswordBox passwordBox)
        {
            VerifyControlHasName(passwordBox, "passwordBox");

            page.State[passwordBox.Name + ".Password"] = passwordBox.Password;
        }

        public static void SaveState(PhoneApplicationPage page, Pivot pivot)
        {
            VerifyControlHasName(pivot, "pivot");

            page.State[pivot.Name + ".SelectedIndex"] = pivot.SelectedIndex;
        }

        public static void RestoreState(PhoneApplicationPage page)
        {
            foreach (PropertyInfo tombstoneProperty in FindTombstoneProperties(page.DataContext))
            {
                string key = "ViewModel." + tombstoneProperty.Name;

                if (page.State.ContainsKey(key))
                {
                    tombstoneProperty.SetValue(page.DataContext, page.State[key], null);
                }
            }

            if (page.State.ContainsKey("FocusedControl.Name"))
            {
                string focusedControlName = (string)page.State["FocusedControl.Name"];
                Control focusedControl = (Control)page.FindName(focusedControlName);

                if (focusedControl != null)
                {
                    page.Loaded += delegate
                    {
                        focusedControl.Focus();
                    };
                }
            }
        }

        public static void RestoreState(PhoneApplicationPage page, ListBox listBox)
        {
            VerifyControlHasName(listBox, "listBox");

            string key = listBox.Name + ".VerticalOffset";

            if (page.State.ContainsKey(key))
            {
                double offset = (double)page.State[key];

                listBox.Loaded += delegate
                {
                    ScrollViewer viewer = (ScrollViewer)(VisualTreeHelper.GetChild(listBox, 0) as FrameworkElement).FindName("ScrollViewer");
                    viewer.ScrollToVerticalOffset((double)offset);
                };
            }
        }

        public static void RestoreState(PhoneApplicationPage page, CheckBox checkBox)
        {
            VerifyControlHasName(checkBox, "checkBox");

            string key = checkBox.Name + ".IsChecked";

            if (page.State.ContainsKey(key))
            {
                bool isChecked = (bool)page.State[key];

                checkBox.Loaded += delegate
                {
                    checkBox.IsChecked = isChecked;
                };
            }
        }

        public static void RestoreState(PhoneApplicationPage page, ToggleSwitch toggleSwitch)
        {
            VerifyControlHasName(toggleSwitch, "toggleSwitch");

            string key = toggleSwitch.Name + ".IsChecked";

            if (page.State.ContainsKey(key))
            {
                bool isChecked = (bool)page.State[key];

                toggleSwitch.Loaded += delegate
                {
                    toggleSwitch.IsChecked = isChecked;
                };
            }
        }

        public static void RestoreState(PhoneApplicationPage page, TextBox textBox)
        {
            VerifyControlHasName(textBox, "textBox");

            string key = textBox.Name + ".Text";

            if (page.State.ContainsKey(key))
            {
                string text = (string)page.State[key];
                int selectionStart = (int)page.State[textBox.Name + ".SelectionStart"];
                int selectionLength = (int)page.State[textBox.Name + ".SelectionLength"];

                textBox.Loaded += delegate
                {
                    textBox.Text = text;
                    textBox.SelectionStart = selectionStart;
                    textBox.SelectionLength = selectionLength;
                };
            }
        }

        public static void RestoreState(PhoneApplicationPage page, PasswordBox passwordBox)
        {
            VerifyControlHasName(passwordBox, "passwordBox");

            string key = passwordBox.Name + ".Password";

            if (page.State.ContainsKey(key))
            {
                string password = (string)page.State[key];

                passwordBox.Loaded += delegate
                {
                    passwordBox.Password = password;
                };
            }
        }

        public static void RestoreState(PhoneApplicationPage page, Pivot pivot)
        {
            VerifyControlHasName(pivot, "pivot");

            string key = pivot.Name + ".SelectedIndex";

            if (page.State.ContainsKey(key))
            {
                int selectedIndex = (int)page.State[key];

                pivot.Loaded += delegate
                {
                    if(pivot.SelectedIndex != selectedIndex)
                        pivot.SelectedIndex = selectedIndex;
                };
            }
        }

        private static void VerifyControlHasName(Control control, string paramName)
        {
            if (string.IsNullOrEmpty(control.Name))
                throw new ArgumentException("You can only save the state of control with a name assigned to them.", paramName);
        }

        private static IEnumerable<PropertyInfo> FindTombstoneProperties(object o)
        {
            IList<PropertyInfo> tombstoneProperties = (from p in o.GetType().GetProperties()
                                                            where p.GetCustomAttributes(typeof(TombstoneAttribute), false).Length > 0
                                                            select p).ToList();

            foreach (PropertyInfo tombstoneProperty in tombstoneProperties)
            {
                if (!tombstoneProperty.CanRead || !tombstoneProperty.CanWrite)
                {
                    throw new TombstoneException("The getter and the setter of a property that needs to be tomstoned must be declared public.");
                }
            }

            return tombstoneProperties;
        }
    }
}
