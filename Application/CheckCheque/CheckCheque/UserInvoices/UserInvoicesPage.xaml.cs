﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace CheckCheque.UserInvoices
{
    public partial class UserInvoicesPage : ContentPage
    {
        public UserInvoicesPage()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, string.Empty);
        }
    }
}
