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
using Ninject;
using CloudFox.Presentation.ViewModels;
using System.Windows.Data;
using System.Windows.Navigation;
using CloudFox.Presentation.Util;

namespace CloudFox.Presentation.Views
{
    public partial class SettingPage : PhoneApplicationPage
    {
        private bool isNewPage;

        public SettingPage()
        {
            InitializeComponent();

            DataContext = App.Kernel.Get<SettingsViewModel>();

            isNewPage = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (isNewPage)
            {
                TombstoneHelper.RestoreState(this);
                TombstoneHelper.RestoreState(this, accountTextBox);
                TombstoneHelper.RestoreState(this, passwordPasswordBox);
                TombstoneHelper.RestoreState(this, passphrasePasswordBox);
                TombstoneHelper.RestoreState(this, useDefaultServerCheckBox);
                TombstoneHelper.RestoreState(this, serverTextBox);
                TombstoneHelper.RestoreState(this, bookmarksToggleSwitch);
                TombstoneHelper.RestoreState(this, historyToggleSwitch);
                TombstoneHelper.RestoreState(this, tabsToggleSwitch);

                isNewPage = false;
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);

            TombstoneHelper.SaveState(this);
            TombstoneHelper.SaveState(this, accountTextBox);
            TombstoneHelper.SaveState(this, passwordPasswordBox);
            TombstoneHelper.SaveState(this, passphrasePasswordBox);
            TombstoneHelper.SaveState(this, useDefaultServerCheckBox);
            TombstoneHelper.SaveState(this, serverTextBox);
            TombstoneHelper.SaveState(this, bookmarksToggleSwitch);
            TombstoneHelper.SaveState(this, historyToggleSwitch);
            TombstoneHelper.SaveState(this, tabsToggleSwitch);
        }

        private void DoneIconButton_Click(object sender, EventArgs e)
        {
            SettingsViewModel vm = (SettingsViewModel)DataContext;
            vm.Save.Execute(null);
        }

        private void CancelIconButton_Click(object sender, EventArgs e)
        {
            SettingsViewModel vm = (SettingsViewModel)DataContext;
            vm.Cancel.Execute(null);
        }

        private void CheckBox_Clicked(object sender, RoutedEventArgs e)
        {
            // Force the update of the VM
            BindingExpression binding = GetBindingExpression(sender);
            if (binding != null)
                binding.UpdateSource();
        }

        private BindingExpression GetBindingExpression(object sender)
        {
            TextBox textBox = sender as TextBox;
            if (textBox != null)
                return textBox.GetBindingExpression(TextBox.TextProperty);
            else
            {
                PasswordBox passwordBox = sender as PasswordBox;
                if (passwordBox != null)
                    return passwordBox.GetBindingExpression(PasswordBox.PasswordProperty);
                else
                {
                    CheckBox checkBox = sender as CheckBox;
                    if (checkBox != null)
                        return checkBox.GetBindingExpression(CheckBox.IsCheckedProperty);
                    else
                        return null;
                }
            }
        }

        // Temporary until Prism supports WP 7.1
        private void HelpIconButton_Click(object sender, EventArgs e)
        {
            ((SettingsViewModel)this.DataContext).Help.Execute(null);
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            GoogleAnalytics.EasyTracker.GetTracker().SendCurrentView();
        }
    }
}