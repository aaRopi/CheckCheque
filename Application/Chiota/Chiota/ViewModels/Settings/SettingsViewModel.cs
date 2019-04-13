﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Chiota.Exceptions;
using Chiota.Extensions;
using Chiota.Models;
using Chiota.Models.Binding;
using Chiota.Services.Share;
using Chiota.ViewModels.Base;
using Chiota.Views.Settings;
using Xamarin.Forms;

namespace Chiota.ViewModels.Settings
{
    public class SettingsViewModel : BaseViewModel
    {
        #region Attributes

        private List<SettingsBinding> _settingsList;

        #endregion

        #region Properties

        public List<SettingsBinding> SettingsList
        {
            get => _settingsList;
            set
            {
                _settingsList = value;
                OnPropertyChanged(nameof(SettingsList));
            }
        }

        #endregion

        #region Constructors

        public SettingsViewModel()
        {
            //Init the settings list.
            SettingsList = new List<SettingsBinding>();
        }

        #endregion

        #region Init

        public override void Init(object data = null)
        {
            base.Init(data);

            InitView();
        }

        #endregion

        #region Methods

        #region InitView

        private void InitView()
        {
            var tmp = new List<SettingsBinding>
            {
                new SettingsBinding(SettingsItemType.Profile, "Profile", ImageSource.FromFile("account.png")),
                new SettingsBinding(SettingsItemType.InviteFriends, "Invite friends", ImageSource.FromFile("friends.png")),
                new SettingsBinding(SettingsItemType.About, "About", ImageSource.FromFile("help.png"))
            };

            SettingsList = tmp;
        }

        #endregion

        #endregion

        #region Commands

        #region Tap

        public ICommand TapCommand
        {
            get
            {
                return new Command(async (param) =>
                {
                    if (!(param is SettingsBinding settingsItem)) return;

                    switch (settingsItem.Type)
                    {
                        case SettingsItemType.Profile:

                            await PushAsync<ProfileView>();

                            break;
                        case SettingsItemType.InviteFriends:

                            var title = "Chiota";
                            var message = "You should check out Chiota, Your IOTA Chat App";
                            var url = "https://play.google.com/store/apps/details?id=chiotaapp.chiotaapp";

                            await CrossShare.Current.Share(new ShareMessage()
                            {
                                Title = title,
                                Text = message,
                                Url = url
                            });

                            break;
                        case SettingsItemType.About:

                            await PushAsync<AboutView>();

                            break;
                        default:
                            await new UnknownException(new ExcInfo()).ShowAlertAsync();
                            break;
                    }
                });
            }
        }

        #endregion

        #endregion
    }
}
