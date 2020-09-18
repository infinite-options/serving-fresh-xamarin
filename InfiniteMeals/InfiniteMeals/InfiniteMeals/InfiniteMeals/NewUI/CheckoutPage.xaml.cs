using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals.NewUI
{
    public partial class CheckoutPage : ContentPage
    {
        public CheckoutPage()
        {
            NavigationPage.SetBackButtonTitle(this, "Farms");
            InitializeComponent();
        }
        public void onTap(object sender, EventArgs e)
        {

        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new logInPage();
        }

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void ImageButton_Clicked_1(System.Object sender, System.EventArgs e)
        {
            
        }

        void ImageButton_Clicked_2(System.Object sender, System.EventArgs e)
        {
            
        }

        void ImageButton_Clicked_3(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new profileUser();
        }
    }
}
