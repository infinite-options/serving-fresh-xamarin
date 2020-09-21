using System;
using System.Collections.Generic;
using Xamarin.Forms;
using InfiniteMeals.NewUI;

namespace InfiniteMeals
{
    public partial class profileUser : ContentPage
    {
        static bool state = false;
        public profileUser()
        {
            InitializeComponent();
        }

        void Entry_PropertyChanged(System.Object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        //void Button_Clicked(System.Object sender, System.EventArgs e)
        //{
        //    string password = "infiniteOptions";
        //    string startPassword = "*************";
        //    if (!state)
        //    {
        //        passwordUser.Text = password;
        //        state = true;
        //    }
        //    else
        //    {
        //        passwordUser.Text = startPassword;
        //        state = false;
        //    }
        //}

        //void Button_Clicked_1(System.Object sender, System.EventArgs e)
        //{
        //    string password = "infiniteOptions";
        //    string startPassword = "*************";
        //    if (!state)
        //    {
        //        confirmPassowordUser.Text = password;
        //        state = true;
        //    }
        //    else
        //    {
        //        confirmPassowordUser.Text = startPassword;
        //        state = false;
        //    }
        //}

        void Days(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new businessPage();
        }

        async void Orders(System.Object sender, System.EventArgs e)
        {
        }

        async void Info(System.Object sender, System.EventArgs e)
        {
        }

        async void Profile(System.Object sender, System.EventArgs e)
        {
        }

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new CheckoutPage();
        }

        void InfoClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new InfoPage();
        }

        void ProfileClick(System.Object sender, System.EventArgs e)
        {
            // AGAIN SINCE YOU ARE IN THE PROFILE PAGE NOTHING SHOULD HAPPEN
            // WHEN CLICK
        }
    }
}
