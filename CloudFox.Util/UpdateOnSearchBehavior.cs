using System;
using System.Net;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using Microsoft.Phone.Controls;

namespace CloudFox.Util
{
    public class UpdateOnSearchBehavior: Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.KeyDown += new KeyEventHandler(AssociatedObject_KeyDown);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.KeyDown -= new KeyEventHandler(AssociatedObject_KeyDown);
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                BindingExpression binding = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();

                // Set the focus back to the page instead of the TextBox
                PhoneApplicationPage currentPage = ((PhoneApplicationFrame)Application.Current.RootVisual).Content as PhoneApplicationPage;
                if (currentPage != null)
                    currentPage.Focus();
            }
        }
    }
}
