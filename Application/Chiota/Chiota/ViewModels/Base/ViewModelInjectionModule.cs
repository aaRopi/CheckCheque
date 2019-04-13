﻿using Autofac;
using Chiota.ViewModels.Authentication;
using Chiota.ViewModels.BackUp;
using Chiota.ViewModels.Chat;
using Chiota.ViewModels.Contact;
using Chiota.ViewModels.Help;
using Chiota.ViewModels.Settings;
using Chiota.ViewModels.Tabbed;

namespace Chiota.ViewModels.Base
{
    public class ViewModelInjectionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LogInViewModel>().PropertiesAutowired();
            builder.RegisterType<NewSeedViewModel>().PropertiesAutowired();
            builder.RegisterType<SetPasswordViewModel>().PropertiesAutowired();
            builder.RegisterType<SetSeedViewModel>().PropertiesAutowired();
            builder.RegisterType<SetUserViewModel>().PropertiesAutowired();
            builder.RegisterType<WelcomeViewModel>().PropertiesAutowired();

            builder.RegisterType<BackUpViewModel>().PropertiesAutowired();
            builder.RegisterType<ConfirmSeedViewModel>().PropertiesAutowired();
            builder.RegisterType<PaperCopyViewModel>().PropertiesAutowired();
            builder.RegisterType<QrCodeViewModel>().PropertiesAutowired();
            builder.RegisterType<WriteSeedViewModel>().PropertiesAutowired();

            builder.RegisterType<SeedHelpViewModel>().PropertiesAutowired();

            builder.RegisterType<TabbedNavigationViewModel>().PropertiesAutowired();

            builder.RegisterType<ChatsViewModel>().PropertiesAutowired();
            builder.RegisterType<ChatViewModel>().PropertiesAutowired();

            builder.RegisterType<ContactAddressViewModel>().PropertiesAutowired();
            builder.RegisterType<AddContactViewModel>().PropertiesAutowired();
            builder.RegisterType<ContactsViewModel>().PropertiesAutowired();
            builder.RegisterType<ContactRequestViewModel>().PropertiesAutowired();

            builder.RegisterType<ProfileViewModel>().PropertiesAutowired();
            builder.RegisterType<AboutViewModel>().PropertiesAutowired();

            builder.RegisterType<SettingsViewModel>().PropertiesAutowired();
        }
    }
}