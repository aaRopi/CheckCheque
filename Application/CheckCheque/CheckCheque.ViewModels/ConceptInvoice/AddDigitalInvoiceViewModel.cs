using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckCheque.Core.Enums;
using CheckCheque.Core.Repositories.Interfaces;
using CheckCheque.Core.Services.Interfaces;
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

        private bool _showParsedInvoiceDetails;
        public bool ShowParsedInvoiceDetails
        {
            get => _showParsedInvoiceDetails;
            private set
            {
                if (_showParsedInvoiceDetails != value)
                {
                    _showParsedInvoiceDetails = value;

                    InvoiceAmount = $"{Invoice.Amount}";
                    InvoiceBankAccountNumber = Invoice.BankAccountNumber;
                    InvoiceIssuerAddress = Invoice.IssuerAddress;
                    InvoiceKvkNumber = Invoice.KvkNumber;

                    RaisePropertyChanged(nameof(ShowParsedInvoiceDetails));
                }
            }
        }

        private string _invoiceName;
        public string InvoiceName
        {
            get => _invoiceName;
            set
            {
                if (_invoiceName != value)
                {
                    _invoiceName = value;
                    RaisePropertyChanged(nameof(InvoiceName));
                }
            }
        }

        private string _invoiceAmount;
        public string InvoiceAmount
        {
            get => _invoiceAmount;
            set
            {
                if (_invoiceAmount != value)
                {
                    _invoiceAmount = value;
                    RaisePropertyChanged(nameof(InvoiceAmount));
                }
            }
        }

        private string _invoiceBankAccountNumber;
        public string InvoiceBankAccountNumber
        {
            get => _invoiceBankAccountNumber;
            set
            {
                if (_invoiceBankAccountNumber != value)
                {
                    _invoiceBankAccountNumber = value;
                    RaisePropertyChanged(nameof(InvoiceBankAccountNumber));
                }
            }
        }

        private string _invoiceIssuerAddress;
        public string InvoiceIssuerAddress
        {
            get => _invoiceIssuerAddress;
            set
            {
                if (_invoiceIssuerAddress != value)
                {
                    _invoiceIssuerAddress = value;
                    RaisePropertyChanged(nameof(InvoiceIssuerAddress));
                }
            }
        }

        private string _invoiceKvkNumber;
        public string InvoiceKvkNumber
        {
            get => _invoiceKvkNumber;
            set
            {
                if (_invoiceKvkNumber != value)
                {
                    _invoiceKvkNumber = value;
                    RaisePropertyChanged(nameof(InvoiceKvkNumber));
                }
            }
        }

        public IInvoiceService InvoiceService { get; private set; }
        public IInvoicesRepository InvoicesRepository { get; private set; }

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

            if (methodOfSelection.Equals("Camera", StringComparison.InvariantCultureIgnoreCase))
            {
                filePath = await TakePhotoAsync();
                if (filePath == null)
                {
                    return;
                }

                Invoice.FileType = DigitalInvoiceFileType.Image;
                _ = Task.Run(async () => { await ParseImageForInvoiceDataAsync(filePath); });
            }

            if (methodOfSelection.Equals("FileSystem", StringComparison.InvariantCultureIgnoreCase))
            {
                filePath = await SelectFileFromFileStorageAsync();
                if (filePath == null)
                {
                    return;
                }

                Invoice.FileType = DigitalInvoiceFileType.Image;
                _ = Task.Run(async () => { await ParseFileForInvoiceDataAsync(filePath); });
            }

            Invoice.FileName = filePath;
        });

        public ICommand InvoiceFileSelectedCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(Invoice.FileName))
            {
                await CoreMethods.DisplayAlert("Error", "Please select a file for the invoice first", "Ok");
            }

            Invoice.Name = InvoiceName;
            Invoice.Amount = Double.Parse(InvoiceAmount);
            Invoice.BankAccountNumber = InvoiceBankAccountNumber;
            Invoice.KvkNumber = InvoiceKvkNumber;
            Invoice.IssuerAddress = InvoiceIssuerAddress;

            InvoicesRepository.AddOrUpdateInvoice(Invoice);

            await CoreMethods.PushPageModel<InvoiceSelectedViewModel>(Invoice);
        });

        public ICommand DismissInvoiceOperationCommand => new Command(async () =>
        {
            await CoreMethods.PopModalNavigationService();
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

            InvoiceService = DependencyService.Get<IInvoiceService>();
            InvoicesRepository = DependencyService.Get<IInvoicesRepository>();
        }

        protected override void ViewIsDisappearing(object sender, EventArgs e)
        {
            base.ViewIsDisappearing(sender, e);
        }

        public override void ReverseInit(object returnedData)
        {
            base.ReverseInit(returnedData);
        }

        private async Task ParseFileForInvoiceDataAsync(string filePath)
        {
            if (InvoiceService == null)
            {
                return;
            }

            try
            {
                var invoice = await InvoiceService.ParseInvoiceDataFromFile(filePath);
                if (invoice != null)
                {
                    Invoice.Amount = invoice.Amount;
                    Invoice.BankAccountNumber = invoice.BankAccountNumber;
                    Invoice.KvkNumber = invoice.KvkNumber;
                    Invoice.IssuerAddress = invoice.IssuerAddress;
                }
            }
            catch (Exception ex)
            {
                await CoreMethods.DisplayAlert("Error", ex.Message, "Ok");
            }
            finally
            {
                ShowParsedInvoiceDetails = true;
            }
        }

        private async Task ParseImageForInvoiceDataAsync(string filePath)
        {
            if (InvoiceService != null)
            {
                try
                {
                    var invoice = await InvoiceService.ParseInvoiceDataFromImage(filePath);
                    if (invoice != null)
                    {
                        Invoice.Amount = invoice.Amount;
                        Invoice.BankAccountNumber = invoice.BankAccountNumber;
                        Invoice.KvkNumber = invoice.KvkNumber;
                        Invoice.IssuerAddress = invoice.IssuerAddress;
                    }
                }
                catch (Exception ex)
                {
                    await CoreMethods.DisplayAlert("Error", ex.Message, "Ok");
                }
                finally
                {
                    ShowParsedInvoiceDetails = true;
                }
            }
        }

        // TODO: this can be abstracted away to a separate project even, like Infrastructure.Framework
        // thus, this project will not need the CrossMedia or CrossFilePicker packages.
        private async Task<string> TakePhotoAsync()
        {
            await CrossMedia.Current.Initialize();

            if ((!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported) && !CrossMedia.Current.IsPickPhotoSupported)
            {
                await CoreMethods.DisplayAlert("Error: No Camera or photos", ":( No camera or photos available.", "Ok");
                return null;
            }

            Plugin.Media.Abstractions.MediaFile file = null;

            if (CrossMedia.Current.IsCameraAvailable)
            {
                file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "InvoicePhotos",
                    Name = Invoice.Name
                });
            }
            else if (CrossMedia.Current.IsPickPhotoSupported)
            {
                file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (file == null)
            {
                await CoreMethods.DisplayAlert("Error: Something went wrong", "Could not take photo.", "Ok");
                return null;
            }

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
