using FreshMvvm;

namespace CheckCheque.ViewModels.Others
{
    public class ShowInstructionsViewModel : FreshBasePageModel
    {
        public const string ShowScanningInstructions = "ShowScanningInstructions";

        private string _lottieInstructionsFileName;
        public string LottieInstructionsFileName
        {
            get => _lottieInstructionsFileName;

            set
            {
                if (_lottieInstructionsFileName != value)
                {
                    _lottieInstructionsFileName = value;
                    RaisePropertyChanged(nameof(LottieInstructionsFileName));
                }
            }
        }

        private string _title;
        public string Title
        {
            get => _title;

            set
            {
                if (_title != value)
                {
                    _title = value;
                    RaisePropertyChanged(nameof(Title));
                }
            }
        }

        public override void Init(object initData)
        {
            base.Init(initData);

            var whichInstructions = initData as string;
            if (!string.IsNullOrEmpty(whichInstructions))
            {
                if (whichInstructions == ShowScanningInstructions)
                {
                    LottieInstructionsFileName = "lottie_camera_invoice_scan.json";
                    Title = "How to scan invoices?";
                }
            }
        }
    }
}
