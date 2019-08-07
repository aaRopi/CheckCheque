using Xamarin.Forms;

namespace CheckCheque.Models.Settings
{
    public class Setting : BindableObject
    {
        private Command _command;
        private ImageSource _imageSource;
        private string _name;

        public Command Command
        {
            get => _command;
            set
            {
                if (value != _command)
                {
                    _command = value;
                    OnPropertyChanged();
                }
            }
        }

        public ImageSource ImageSource
        {
            get => _imageSource;
            set
            {
                if (value != _imageSource)
                {
                    _imageSource = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (value != _name)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
