using System;
using System.Net;
using Ninject.Modules;
using CloudFox.Presentation.Util;
using CloudFox.Presentation.ViewModels;

namespace CloudFox.Presentation
{
    public class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            // Services
            Bind<IStorage>().To<Storage>().InSingletonScope();
            Bind<INavigationService>().To<NavigationService>();
            Bind<IMessageService>().To<MessageService>();
            Bind<ISynchronizer>().To<Synchronizer>();

            // ViewModels
            Bind<MainViewModel>().ToSelf().InSingletonScope();
        }
    }
}
