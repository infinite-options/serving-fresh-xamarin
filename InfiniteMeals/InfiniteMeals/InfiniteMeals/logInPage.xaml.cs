using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class logInPage : ContentPage
    {
        // WE NEED END POINT TO CHECK IF THE WE HAVE A MATCH EMAIL AND VERIFIED THEIR PASSWORD
        public logInPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        void LogInUser(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.MainPage();
        }

        void SignUpUser(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new signUpPage();
        }

        void ProceedAsGuestUser(System.Object sender, System.EventArgs e)
        { 

            //https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/shell/navigation
            // NEEDS TO IMPLEMENT WITH NAVIGATION PAGE CONSTRUCTOR
            Application.Current.MainPage = new NewUI.MainPage();
        }
    }
}
