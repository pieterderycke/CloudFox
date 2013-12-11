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
    public class BookmarkViewModel
    {
        public BookmarkViewModel(Bookmark bookmark)
        {
            Bookmark = bookmark;
        }

        public Bookmark Bookmark { get; private set; }

        public string Title 
        {
            get
            {
                return Bookmark.Title;
            }
        }

        public string Location 
        {
            get
            {
                return Bookmark.Location;
            }
        }
    }
}
