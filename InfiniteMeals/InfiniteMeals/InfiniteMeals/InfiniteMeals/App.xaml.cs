using InfiniteMeals.Kitchens.Controller;
using InfiniteMeals.PromptAddress;
using InfiniteMeals.Refund;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace InfiniteMeals
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            if (Application.Current.Properties.ContainsKey("zip") && Application.Current.Properties["zip"] != null)
            {
                MainPage = new TabbedMainPage()
                {

                    //BarBackgroundColor = Color.FromHex("#a0050f"),
                    //BarTextColor = Color.White
                };
                
            }
            else
            {
                MainPage = new logInPage();
                {
                    //BarBackgroundColor = Color.FromHex("#a0050f"),
                    //BarTextColor = Color.White
                };
            }
            //MainPage = new TabbedMainPage();
            //MainPage = new NavigationPage(new MainPage());
            //MainPage = new MainPage();
            MainPage = new logInPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
