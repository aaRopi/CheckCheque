using System.Collections.ObjectModel;
using CheckCheque.Models.Settings;
using Xamarin.Forms;

namespace CheckCheque.ViewModels
{
    public class SettingsViewModel : BindableObject
    {
        private ObservableCollection<Settings> _settings;

        public ObservableCollection<Settings> Settings
        {
            get => _settings;
            set
            {
                if (value != _settings)
                {
                    _settings = value;
                    OnPropertyChanged();
                }
            }
        }

        public SettingsViewModel()
        {
            this.Settings = new ObservableCollection<Settings>();
        }
    }
}