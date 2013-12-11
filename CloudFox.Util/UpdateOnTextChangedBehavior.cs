using System;
using System.Windows.Interactivity;
using System.Windows.Controls;
using System.Windows.Data;

namespace CloudFox.Util
{
    /// <summary>
    /// A <see cref="TextBox"/> <see cref="Behavior"/> that triggers the bindings directly when text is changed.
    /// </summary>
    public class UpdateOnTextChangedBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.TextChanged += new TextChangedEventHandler(AssociatedObject_TextChanged);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.TextChanged -= new TextChangedEventHandler(AssociatedObject_TextChanged);
        }

        private void AssociatedObject_TextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression binding = this.AssociatedObject.GetBindingExpression(TextBox.TextProperty);
            binding.UpdateSource();
        }
    }
}
