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
            Navigation.PushAsync(new CheckoutPage());
        }
        public void openHistory(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
    }
}
