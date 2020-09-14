using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class signUpPage : ContentPage
    {
        public signUpPage()
        {
            InitializeComponent();
        }

        // SignUpNewUser
        void SignUpNewUser(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.MainPage();
        }
    }
}
