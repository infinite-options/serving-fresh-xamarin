using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class SocialLoginPage : ContentPage
    {
        public SocialLoginPage()
        {
            InitializeComponent();
            this.BindingContext = new SocialLoginPageViewModel();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
        }
    }
}
