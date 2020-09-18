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
            Application.Current.MainPage = new profileUser();
        }

        void CardClick(System.Object sender, System.EventArgs e)
        {
            // AGAIN NO ACTION NEEDED
        }
    }
}
