﻿using System;
using Xamarin.Forms;

namespace CheckCheque.Views
{
    public partial class VerifyInvoicesPage : ContentPage
    {
        public VerifyInvoicesPage()
        {
            InitializeComponent();
        }

        void OnClicked(object sender, EventArgs eventArgs)
        {
            if (sender == ShowMeHowButton)
            {
                Console.WriteLine("show lottie animation how");
                return;
            }

            if (sender == OpenCameraButton)
            {
                Console.WriteLine("open the camera");
                return;
            }
        }
    }
}
