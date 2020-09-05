using System;
using System.Collections.Generic;
using Plugin.GoogleClient;
using Plugin.FacebookClient;

using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class HomePage : ContentPage
    {
        public HomePage(NetworkAuthData networkAuthData)
        {
            BindingContext = networkAuthData;
            InitializeComponent();
        }

        async void OnLogout(object sender, System.EventArgs e)
        {
            if (BindingContext is NetworkAuthData data)
            {
                switch (data.Name)
                {
                    case "Facebook":
                        CrossFacebookClient.Current.Logout();
                        break;
                    case "Google":
                        CrossGoogleClient.Current.Logout();
                        break;
                }

                await Navigation.PopModalAsync();
            }
        }
    }
}
