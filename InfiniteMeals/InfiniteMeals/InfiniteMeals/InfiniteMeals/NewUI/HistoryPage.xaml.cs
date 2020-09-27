using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals.NewUI
{
    public partial class HistoryPage : ContentPage
    {
        public HistoryPage()
        {
            InitializeComponent();
        }
        public void onTap(object sender, EventArgs e)
        {

        }
        public void openCheckout(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CheckoutPage());
        }
        public void openRefund(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RefundPage());
        }
    }
}
