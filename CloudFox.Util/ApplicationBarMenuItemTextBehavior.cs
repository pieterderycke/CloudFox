using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CloudFox.Util
{
    public class ApplicationBarMenuItemTextBehavior : Behavior<PhoneApplicationPage>
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ApplicationBarMenuItemTextBehavior), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Id { get; set; }

        protected override void OnAttached()
        {
            IApplicationBarMenuItem menuItem = FindApplicationBarMenuItem(Id);
            menuItem.Text = Text;
        }

        private IApplicationBarMenuItem FindApplicationBarMenuItem(string id)
        {
            IApplicationBarMenuItem button = this.AssociatedObject.ApplicationBar.MenuItems.Cast<IApplicationBarMenuItem>().
                Where(b => b.Text.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (button == null)
                throw new ArgumentException("No ApplicationBar menu item exists with the provided name.", "id");
            else
                return button;
        }
    }
}
