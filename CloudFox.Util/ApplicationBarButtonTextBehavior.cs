using System;
using System.Linq;
using System.Windows;
using System.Windows.Interactivity;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace CloudFox.Util
{
    public class ApplicationBarButtonTextBehavior : Behavior<PhoneApplicationPage>
    {
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ApplicationBarButtonTextBehavior), null);

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string Id { get; set; }

        protected override void OnAttached()
        {
            IApplicationBarIconButton button = FindApplicationBarButton(Id);
            button.Text = Text;
        }

        private IApplicationBarIconButton FindApplicationBarButton(string id)
        {
            IApplicationBarIconButton button = this.AssociatedObject.ApplicationBar.Buttons.Cast<IApplicationBarIconButton>().
                Where(b => b.Text.Equals(id, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault();

            if (button == null)
                throw new ArgumentException("No ApplicationBar button exists with the provided name.", "id");
            else
                return button;
        }
    }
}
