﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using CheckCheque.Core.Enums;
using CheckCheque.Core.Repositories.Interfaces;
using CheckCheque.Core.Services.Interfaces;
using CheckCheque.Enums;
using CheckCheque.Models;
using CheckCheque.Models.Settings;
using CheckCheque.ViewModels.Others;
using FreshMvvm;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Plugin.Media;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace CheckCheque.ViewModels.ConceptInvoice
{
    public class AddDigitalInvoiceViewModel: FreshBasePageModel
    {
        #region Properties
        private bool _userHasAllowedInvoiceDataUse;
        public bool UserHasAllowedInvoiceDataUse
        {
            get => _userHasAllowedInvoiceDataUse;
            set
            {
                if (_userHasAllowedInvoiceDataUse != value)
                {
                    _userHasAllowedInvoiceDataUse = value;
                    RaisePropertyChanged(nameof(UserHasAllowedInvoiceDataUse));
                }
            }
        }

        private bool _invoiceOperationIsSuccessful;
        public bool InvoiceOperationIsSuccessful
        {
            get => _invoiceOperationIsSuccessful;
            set
            {
                if (_invoiceOperationIsSuccessful != value)
                {
                    _invoiceOperationIsSuccessful = value;
                    RaisePropertyChanged(nameof(InvoiceOperationIsSuccessful));
                }
            }
        }

        private string _invoiceOperationResult;
        public string InvoiceOperationResult
        {
            get => _invoiceOperationResult;
            set
            {
                if (_invoiceOperationResult != value)
                {
                    _invoiceOperationResult = value;
                    RaisePropertyChanged(nameof(InvoiceOperationResult));
                }
            }
        }

        private bool _showInvoiceOperationStatus;
        public bool ShowInvoiceOperationStatus
        {
            get => _showInvoiceOperationStatus;
            set
            {
                if (_showInvoiceOperationStatus != value)
                {
                    _showInvoiceOperationStatus = value;
                    RaisePropertyChanged(nameof(ShowInvoiceOperationStatus));
                }
            }
        }

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

                    InvoiceNumber = Invoice.InvoiceNumber;
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

        private string _invoiceNumber;
        public string InvoiceNumber
        {
            get => _invoiceNumber;
            set
            {
                if (_invoiceNumber != value)
                {
                    _invoiceNumber = value;
                    RaisePropertyChanged(nameof(InvoiceNumber));
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

        private bool _showBusyStatus;
        public bool ShowBusyStatus
        {
            get => _showBusyStatus;
            set
            {
                if (_showBusyStatus != value)
                {
                    _showBusyStatus = value;
                    RaisePropertyChanged(nameof(ShowBusyStatus));
                }
            }
        }
        
        public IInvoiceService InvoiceService { get; private set; }
        public IInvoicesRepository InvoicesRepository { get; private set; }
        #endregion Properties

        #region Commands
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

        public ICommand InvoiceVerifyOrSignAndSendCommand => new Command(async () =>
        {
            if (string.IsNullOrEmpty(InvoiceName))
            {
                await CoreMethods.DisplayAlert("Error", "Invoice name cannot be empty", "Ok");
                return;
            }

            Invoice.Name = InvoiceName;
            Invoice.InvoiceNumber = InvoiceNumber;
            Invoice.BankAccountNumber = InvoiceBankAccountNumber;
            Invoice.KvkNumber = InvoiceKvkNumber;
            Invoice.IssuerAddress = InvoiceIssuerAddress;

            InvoicesRepository.AddOrUpdateInvoice(Invoice);

            ShowBusyStatus = true;

            if (Invoice.Reason == InvoiceReason.SignAndSend)
            {
                Invoice.LastSignedAndSent = DateTime.UtcNow;

                var status = await InvoiceService.PublishInvoiceAsync(Invoice);
                ShowBusyStatus = false;

                InvoiceOperationIsSuccessful = status == InvoicePublishStatus.Success;
                InvoiceOperationResult = InvoiceOperationIsSuccessful ? "Sign and Sent!" : "Something went wrong :(";
            }

            if (Invoice.Reason == InvoiceReason.Verify)
            {
                Invoice.LastVerified = DateTime.UtcNow;

                var status = await InvoiceService.VerifyInvoiceAsync(Invoice);
                ShowBusyStatus = false;

                InvoiceOperationIsSuccessful = status == InvoiceVerificationStatus.Success;
                InvoiceOperationResult = InvoiceOperationIsSuccessful ? "Valid Invoice" : "Cannot verify invoice";
            }

            ShowBusyStatus = false;
            ShowInvoiceOperationStatus = true;
            await Task.Delay(2000);
            ShowInvoiceOperationStatus = false;

            await CoreMethods.PopPageModel(true);
        });

        public ICommand DismissInvoiceOperationCommand => new Command(async () =>
        {
            await CoreMethods.PopPageModel(true);
        });
        #endregion Commands

        public override async void Init(object initData)
        {
            base.Init(initData);

            var isValidInvoiceReason = initData is InvoiceReason;
            if (isValidInvoiceReason && (InvoiceReason)initData != InvoiceReason.Concept || (InvoiceReason)initData != InvoiceReason.Unknown)
            {
                Invoice = new Invoice();
                Invoice.Reason = (InvoiceReason)initData;
            }
            else
            {
                throw new ArgumentException($"{nameof(initData)} is not the correct reason type! This viewmodel will be useless then!");
            }

            InvoiceService = DependencyService.Get<IInvoiceService>();
            InvoicesRepository = DependencyService.Get<IInvoicesRepository>();

            try
            {
                var userPermissionInString = await SecureStorage.GetAsync(Settings.UserPermissionForInvoiceDataKey);
                if (string.IsNullOrEmpty(userPermissionInString))
                {
                    try
                    {
                        await SecureStorage.SetAsync(Settings.UserPermissionForInvoiceDataKey, Boolean.FalseString);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not set UserPermissionForInvoiceDataKey to FalseString in SecureStorage: " + ex.Message);
                    }
                    finally
                    {
                        UserHasAllowedInvoiceDataUse = false;
                    }
                }
                else
                {
                    UserHasAllowedInvoiceDataUse = userPermissionInString.Equals(Boolean.TrueString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not retrieve UserPermissionForInvoiceDataKey from SecureStorage: " + ex.Message);
            }
        }

        protected override async void ViewIsAppearing(object sender, EventArgs e)
        {
            base.ViewIsAppearing(sender, e);

            if (Invoice.Reason == InvoiceReason.SignAndSend && !UserHasAllowedInvoiceDataUse)
            {
                // user wants to sign and send, but has not granted us permission to use invoice data
                bool permissionGranted = await CoreMethods.DisplayAlert("Grant Permission",
                    "Do you approve that this invoice number, your company's address, your bank account and your KvK number are used to verify the invoice authenticity?",
                    "Agree", "Deny");

                UserHasAllowedInvoiceDataUse = permissionGranted;

                try
                {
                    await SecureStorage.SetAsync(Settings.UserPermissionForInvoiceDataKey,
                        permissionGranted ? Boolean.TrueString : Boolean.FalseString);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error storing permission granted in SecureStorage " + ex.Message);
                }
                finally
                {
                    if (!permissionGranted)
                        await CoreMethods.PopPageModel(true);
                }
            }
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
                    Invoice.InvoiceNumber = invoice.InvoiceNumber;
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
                        Invoice.InvoiceNumber = invoice.InvoiceNumber;
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
