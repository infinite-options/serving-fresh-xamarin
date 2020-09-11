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
        }

        void LogInUser(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NavigationPage(new businessPage());
        }

        void SignUpUser(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new signUpPage();
        }

        void ProceedAsGuestUser(System.Object sender, System.EventArgs e)
        {
            
            Application.Current.MainPage = new NavigationPage(new logInPage());
            Application.Current.MainPage.Navigation.PushAsync(new businessPage());
        }
    }
}
