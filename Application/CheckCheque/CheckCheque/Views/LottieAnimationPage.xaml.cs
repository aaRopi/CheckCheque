using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class LottieAnimationPage : ContentPage
    {
        private string _animationFileSource;

        public string AnimationFileSource
        {
            get { return _animationFileSource; }
            set
            {
                if (_animationFileSource != value)
                {
                    _animationFileSource = value;
                    OnPropertyChanged(nameof(AnimationFileSource));
                }
            }
        }

        public LottieAnimationPage(string animationFileSource = null, string title = null)
        {
            InitializeComponent();

            if (animationFileSource != null || animationFileSource != string.Empty)
            {
                AnimationFileSource = animationFileSource;
            }

            if (title != null || title != string.Empty)
            {
                Title = title;
            }

        }
    }
}
