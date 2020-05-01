﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RobofestApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CheckSelectionPage : ContentPage
    {
        private static int FieldLoaded;
        public CheckSelectionPage(int Field)
        {
            FieldLoaded = Field;
            InitializeComponent();
        }

        private void Information_Correct(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NotReadyPage(FieldLoaded));
        }
        private void Information_Incorrect(object sender, EventArgs e)
        {
            Navigation.PushAsync(new FieldSelectionPage());
        }
    }
}