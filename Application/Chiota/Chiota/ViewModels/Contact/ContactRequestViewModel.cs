﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Chiota.Exceptions;
using Chiota.Extensions;
using Chiota.Helper;
using Chiota.Services.DependencyInjection;
using Chiota.Services.UserServices;
using Chiota.ViewModels.Base;
using Pact.Palantir.Usecase;
using Pact.Palantir.Usecase.AcceptContact;
using Pact.Palantir.Usecase.DeclineContact;
using Tangle.Net.Entity;
using Xamarin.Forms;

namespace Chiota.ViewModels.Contact
{
    public class ContactRequestViewModel : BaseViewModel
    {
        #region Attributes

        private string _username;
        private ImageSource _profileImageSource;

        private Pact.Palantir.Entity.Contact _contact;

        #endregion

        #region Properties

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public ImageSource ProfileImageSource
        {
            get => _profileImageSource;
            set
            {
                _profileImageSource = value;
                OnPropertyChanged(nameof(ProfileImageSource));
            }
        }

        #endregion

        #region Init

        public override void Init(object data = null)
        {
            base.Init(data);

            _contact = data as Pact.Palantir.Entity.Contact;
            if(_contact == null)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await new UnknownException(new ExcInfo()).ShowAlertAsync();
                    await PopAsync();
                });
                return;
            }

            Username = _contact.Name;

            if(_contact.ImagePath != null)
                ProfileImageSource = ImageSource.FromUri(new Uri(ChiotaConstants.IpfsHashGateway + _contact.ImagePath));
            else
                ProfileImageSource = ImageSource.FromFile("account.png");
        }

        #endregion

        #region Commands

        #region Accept

        public ICommand AcceptCommand
        {
            get
            {
                return new Command(async() =>
                {
                    await PushLoadingSpinnerAsync("Accepting contact");

                    var acceptContactInteractor = DependencyResolver.Resolve<IUsecaseInteractor<AcceptContactRequest, AcceptContactResponse>>();

                    var response = await acceptContactInteractor.ExecuteAsync(new AcceptContactRequest{
                        UserName = UserService.CurrentUser.Name,
                        UserImagePath = UserService.CurrentUser.ImagePath,
                        ChatAddress = new Address(_contact.ChatAddress),
                        ChatKeyAddress = new Address(_contact.ChatKeyAddress),
                        ContactAddress = new Address(_contact.ContactAddress),
                        ContactPublicKeyAddress = new Address(_contact.PublicKeyAddress),
                        UserPublicKeyAddress = new Address(UserService.CurrentUser.PublicKeyAddress),
                        UserKeyPair = UserService.CurrentUser.NtruKeyPair,
                        UserContactAddress = new Address(UserService.CurrentUser.RequestAddress)
                    });

                    await PopPopupAsync();

                    if (response.Code == ResponseCode.Success)
                    {
                        //Update the contact in the database.
                        var contact = Database.Contact.GetContactByChatAddress(_contact.ChatAddress);
                        contact.Accepted = true;
                        Database.Contact.UpdateObject(contact);

                        await DisplayAlertAsync("Successful action", "The contact was successfully added.");
                        await PopAsync();
                    }
                    else
                        await DisplayAlertAsync("Error", $"An error (Code: {(int)response.Code}) occured while adding the contact.");
                });
            }
        }

        #endregion

        #region Decline

        public ICommand DeclineCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await PushLoadingSpinnerAsync("Declining contact");

                    var declineContactInteractor = DependencyResolver.Resolve<IUsecaseInteractor<DeclineContactRequest, DeclineContactResponse>>();

                    var response = await declineContactInteractor.ExecuteAsync(new DeclineContactRequest
                    {
                        ContactChatAddress = new Address(_contact.ChatAddress),
                        UserPublicKeyAddress = new Address(UserService.CurrentUser.PublicKeyAddress)
                    });

                    await PopPopupAsync();

                    if (response.Code == ResponseCode.Success)
                    {
                        //Update the contact in the database.
                        var contact = Database.Contact.GetContactByChatAddress(_contact.ChatAddress);
                        Database.Contact.DeleteObject(contact.Id);

                        await DisplayAlertAsync("Successful action", "The contact was successfully declined.");
                        await PopAsync();
                    }
                    else
                        await DisplayAlertAsync("Error", $"An error (Code: {(int)response.Code}) occured while decline the contact.");
                });
            }
        }

        #endregion

        #endregion
    }
}
