using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckCheque.Models;
using CheckCheque.ViewModels.Others;
using FreshMvvm;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ConceptInvoice
{
    public class AddDigitalInvoiceViewModel: FreshBasePageModel
    {
        private Invoice _invoice;
        public Invoice Invoice
        {
            get => _invoice;
            private set
            {
                if (_invoice != value)
                {
                    _invoice = value;
                    RaisePropertyChanged(nameof(Invoice));
                }
            }
        }

        private string _selectedFileName;
        public string SelectedFileName
        {
            get => _selectedFileName;
            private set
            {
                if (_selectedFileName != value)
                {
                    _selectedFileName = value;
                    RaisePropertyChanged(nameof(SelectedFileName));
                }
            }
        }

        public ICommand ShowScanningInstructionsCommand => new Command(async () =>
        {
            await CoreMethods.PushPageModel<ShowInstructionsViewModel>(ShowInstructionsViewModel.ShowScanningInstructions);
        });

        public ICommand SelectDigitalInvoiceCommand => new Command<string>(async (string methodOfSelection) =>
        {
            if (string.IsNullOrEmpty(methodOfSelection))
            {
                await CoreMethods.DisplayAlert("Error", "Cannot select digital file without specified method", "Ok");
            }

            string filePath = "";

            if (methodOfSelection == "Camera")
            {
                filePath = await TakePhotoAsync();
                if (filePath == null)
                {
                    return;
                }
            }

            if (methodOfSelection == "FileSystem")
            {
                filePath = await SelectFileFromFileStorageAsync();
                if (filePath == null)
                {
                    return;
                }
            }

            Invoice.FileName = filePath;
            SelectedFileName = Invoice.FileName;
        });

        public ICommand InvoiceFileSelectedCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(Invoice.FileName))
            {
                await CoreMethods.DisplayAlert("Error", "Please select a file for the invoice first", "Ok");
            }

            await CoreMethods.PushPageModel<InvoiceSelectedViewModel>(Invoice);
        });

        public override void Init(object initData)
        {
            base.Init(initData);

            var invoice = initData as Invoice;
            if (invoice != null)
            {
                Invoice = invoice;
            }
            else
            {
                throw new ArgumentException($"{nameof(initData)} cannot be null! This viewmodel will be useless then!");
            }
        }

        // TODO: this can be abstracted away to a separate project even, like Infrastructure.Framework
        // thus, this project will not need the CrossMedia or CrossFilePicker packages.
        private async Task<string> TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await CoreMethods.DisplayAlert("Error: No Camera", ":( No camera available.", "Ok");
                return null;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "InvoicePhotos",
                Name = Invoice.Name
            });

            if (file == null)
            {
                await CoreMethods.DisplayAlert("Error: Something went wrong", "Could not take photo.", "Ok");
                return null;
            }

            await CoreMethods.DisplayAlert("Image File Location", file.Path, "Ok");

            return file.Path;
        }

        // TODO: this can be abstracted away to a separate project even, like Infrastructure.Framework
        // thus, this project will not need the CrossMedia or CrossFilePicker packages.
        private async Task<string> SelectFileFromFileStorageAsync()
        {
            try
            {
                FileData fileData = await CrossFilePicker.Current.PickFile();
                if (fileData == null)
                {
                    return null; // user canceled file picking
                }

                string fileName = fileData.FileName;
                string contents = System.Text.Encoding.UTF8.GetString(fileData.DataArray);

                Console.WriteLine("File name chosen: " + fileName);
                Console.WriteLine("File data: " + contents);

                await CoreMethods.DisplayAlert("Digital File Location", fileData.FilePath, "Ok");

                return fileData.FilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception choosing file: " + ex);
            }

            return null;
        }
    }
}
