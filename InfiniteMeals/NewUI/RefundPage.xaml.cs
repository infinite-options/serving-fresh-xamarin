using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals.NewUI
{
    public partial class RefundPage : ContentPage
    {
        public RefundPage()
        {
            InitializeComponent();
        }
        public void onTap(object sender, EventArgs e)
        {

        }
        public void openCheckout(object sender, EventArgs e)
        {
            Application.Current.MainPage = new CheckoutPage();
        }
        public void openHistory(object sender, EventArgs e)
        {
            Application.Current.MainPage = new HistoryPage();
        }
    }
}
