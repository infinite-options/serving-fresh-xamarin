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
            userFirstName.Text = Application.Current.Properties["userFirstName"].ToString();
            userLastName.Text = Application.Current.Properties["userFirstName"].ToString();
            Position position = new Position((double)Application.Current.Properties["latitude"], (double)Application.Current.Properties["longitude"]);
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
            Application.Current.MainPage = new OrdersPage();
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
            Application.Current.Properties["userDeliveryInstructions"] = userDeliveryInstructions.Text;
        }
    }
}
