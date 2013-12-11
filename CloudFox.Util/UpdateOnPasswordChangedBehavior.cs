using System;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows;

namespace CloudFox.Util
{
    /// <summary>
    /// A <see cref="PasswordBox"/> <see cref="Behavior"/> that triggers the bindings directly when password is changed.
    /// </summary>
    public class UpdateOnPasswordChangedBehavior : Behavior<PasswordBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PasswordChanged += new RoutedEventHandler(AssociatedObject_PasswordChanged);
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.PasswordChanged -= new RoutedEventHandler(AssociatedObject_PasswordChanged);
        }

        private void AssociatedObject_PasswordChanged(object sender, System.Windows.RoutedEventArgs e)
        {
            BindingExpression binding = this.AssociatedObject.GetBindingExpression(PasswordBox.PasswordProperty);
            binding.UpdateSource();
        }
    }
}
