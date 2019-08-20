using System.Collections.ObjectModel;
using CheckCheque.Models.Settings;
using Xamarin.Forms;

namespace CheckCheque.ViewModels
{
    public class SettingsViewModel : BindableObject
    {
        private ObservableCollection<Setting> _settings;

        public ObservableCollection<Setting> Settings
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
            this.Settings = new ObservableCollection<Setting>
            {
                new Setting
                {
                    Name = "Sync",
                    //ImageSource = "settings_sync_icon"
                },

                new Setting
                {
                    Name = "Scan",
                    //ImageSource = "settings_scan_icon"
                },

                new Setting
                {
                    Name = "Doc Export",
                    //ImageSource = "settings_docExport_icon"
                },

                new Setting
                {
                    Name = "OCR",
                    //ImageSource = "settings_ocr_icon"
                },

                new Setting
                {
                    Name = "Security & Backup",
                    //ImageSource = "settings_secnback_icon"
                },

                new Setting
                {
                    Name = "Help",
                    //ImageSource = "settings_help_icon"
                },

                new Setting
                {
                    Name = "Feedback",
                    //ImageSource = "settings_feedback_icon"
                }
            };
        }
    }
}