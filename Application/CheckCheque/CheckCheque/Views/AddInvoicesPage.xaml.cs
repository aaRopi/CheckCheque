using System;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class AddInvoicesPage : ContentPage
    {
        public AddInvoicesPage()
        {
            InitializeComponent();
        }

        async void OnClickedAsync(object sender, EventArgs eventArgs)
        {
            if (sender == DemoUIButton)
            {
                await Navigation.PushAsync(new InvoiceSelectedPage());
                return;
            }

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

            if (sender == OpenFilesButton)
            {
                try
                {
                    FileData fileData = await CrossFilePicker.Current.PickFile();
                    if (fileData == null)
                        return; // user canceled file picking

                    string fileName = fileData.FileName;
                    string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);

                    Console.WriteLine("File name chosen: " + fileName);
                    Console.WriteLine("File data: " + contents);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Exception choosing file: " + ex.ToString());
                }

                return;
            }
        }
    }
}
