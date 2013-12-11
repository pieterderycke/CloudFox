using System;
using System.Linq;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;

namespace CloudFox.Presentation.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private IStorage storage;

        private int serverStorageVersion;
        private string weaveNode;

        public ProfileViewModel(IStorage storage)
        {
            this.storage = storage;

            this.Clients = new ObservableCollection<ClientViewModel>();
            this.Collections = new ObservableCollection<CollectionInformationViewModel>();

            // Load the profile from durable storage
            this.storage.LoadProfile();

            ProfileInformation profile = this.storage.Profile;

            if (profile != null)
            {
                this.ServerStorageVersion = profile.ServerStorageVersion;
                this.WeaveNode = profile.WeaveNode;

                foreach (CollectionInformation collectionInformation in profile.Collections.OrderBy(c => c.Name))
                    this.Collections.Add(new CollectionInformationViewModel(collectionInformation));

                foreach (Client client in profile.Clients.OrderBy(c => c.Name))
                    this.Clients.Add(new ClientViewModel(client));
            }
        }

        public int ServerStorageVersion 
        {
            get
            {
                return serverStorageVersion;
            }
            private set
            {
                serverStorageVersion = value;
                RaisePropertyChanged("ServerStorageVersion");
            }
        }

        public string WeaveNode
        {
            get
            {
                return weaveNode;
            }
            private set
            {
                weaveNode = value;
                RaisePropertyChanged("WeaveNode");
            }
        }

        public ObservableCollection<ClientViewModel> Clients { get; private set; }
        public ObservableCollection<CollectionInformationViewModel> Collections { get; private set; }
    }
}
