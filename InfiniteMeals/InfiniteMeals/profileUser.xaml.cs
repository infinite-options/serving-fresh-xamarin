using System;
using System.Collections.Generic;
using Xamarin.Forms;

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

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            string password = "infiniteOptions";
            string startPassword = "*************";
            if (!state)
            {
                passwordUser.Text = password;
                state = true;
            }
            else
            {
                passwordUser.Text = startPassword;
                state = false;
            }
        }

        void Button_Clicked_1(System.Object sender, System.EventArgs e)
        {
            string password = "infiniteOptions";
            string startPassword = "*************";
            if (!state)
            {
                confirmPassowordUser.Text = password;
                state = true;
            }
            else
            {
                confirmPassowordUser.Text = startPassword;
                state = false;
            }
        }
    }
}
