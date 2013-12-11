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

namespace CloudFox.Presentation.ViewModels
{
    public class CollectionInformationViewModel
    {
        public CollectionInformationViewModel(CollectionInformation collectionInformation)
        {
            this.Name = collectionInformation.Name;
            this.Text = string.Format("{0} ({1} items, {2} Kb)", collectionInformation.Name,
                collectionInformation.ItemCount, collectionInformation.Size);
        }

        public string Name { get; private set; }
        public string Text { get; private set; }
    }
}
