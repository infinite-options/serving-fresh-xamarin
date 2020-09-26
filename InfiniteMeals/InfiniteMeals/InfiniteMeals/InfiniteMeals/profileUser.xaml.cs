using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace InfiniteMeals
{
    public partial class profileUser : ContentPage
    {
        static bool state = false;
        public profileUser()
        {
            InitializeComponent();

            userEmailAddress.Text = (string) Application.Current.Properties["userEmailAddress"];
            userFirstName.Text = (string)Application.Current.Properties["userFirstName"];
            userLastName.Text = (string)Application.Current.Properties["userLastName"];

            userAddress.Text = (string)Application.Current.Properties["userAddress"];
            userUnitNumber.Text = (string)Application.Current.Properties["userAddressUnit"];
            userCity.Text = (string)Application.Current.Properties["userCity"]; 
            userState.Text = (string)Application.Current.Properties["userState"];
            userZipcode.Text = (string)Application.Current.Properties["userZipCode"];
            userPhoneNumber.Text = (string)Application.Current.Properties["userPhoneNumber"];

            Position position = new Position(Double.Parse(Application.Current.Properties["latitude"].ToString()), Double.Parse(Application.Current.Properties["longitude"].ToString()));
            map.MapType = MapType.Satellite;
            var mapSpan = new MapSpan(position, 0.000001, 0.000001);
            map.MoveToRegion(mapSpan);
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
            Application.Current.MainPage = new NewUI.CheckoutPage();
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

        void SaveChangesClick(System.Object sender, System.EventArgs e)
        {
            
        }
    }
}
