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
    }
}
