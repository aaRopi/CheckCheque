using System;
using Plugin.Media;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class SendInvoicesPage : ContentPage
    {
        public SendInvoicesPage()
        {
            InitializeComponent();
        }

        async void OnClickedAsync(object sender, EventArgs eventArgs)
        {
            if (sender == ShowMeHowButton)
            {
                Console.WriteLine("show lottie animation how");

                await Navigation.PushAsync(new LottieAnimationPage("lottie_camera_invoice_scan.json", "Scan invoices"));

                return;
            }

            if (sender == OpenCameraButton)
            {
                Console.WriteLine("open the camera");

                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "test.jpg"
                });

                if (file == null)
                    return;

                await DisplayAlert("File Location", file.Path, "OK");

                ImageSource imageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                // TODO do something with the imagesource!

                return;
            }
        }
    }
}
