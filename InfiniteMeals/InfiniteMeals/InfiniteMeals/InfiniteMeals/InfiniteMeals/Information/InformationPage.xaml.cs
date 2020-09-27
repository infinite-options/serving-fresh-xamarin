using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
// using Android.Content;
// using Foundation;
// using UIKit;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//using Android.App;
//using Android.Net;

namespace InfiniteMeals.Information
{

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InformationPage : ContentPage
    {
        public InformationPage()
        {
            InitializeComponent();
            //_ = DisplayGuidAsync();
                        
        }

        private async void DisplayGuidAsync(object sender, EventArgs args)
        {
            var guid = Preferences.Get("guid", "null");
            await DisplayAlert("guid", guid, "OK");
            Console.WriteLine("information guid:" + guid);
        }

        async void NavigateToAppSetting(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("Thank you!", "Make sure you restart the app after you've enabled", "Ok");
            DependencyService.Get<ISettingsHelper>().OpenAppSettings();
            //switch (Device.RuntimePlatform)
            //{
            //    case Device.iOS:
            //        var url = new NSUrl($"app-settings:notifications");
            //        UIApplication.SharedApplication.OpenUrl(url);
            //        break;
            //    case Device.Android:
            //        //Android.App.Application.Context.StartActivity(new Intent(
            //        //    Android.Provider.Settings.ActionApplicationDetailsSettings,
            //        //    Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName)));
            //        Intent intent = new Intent(
            //        Android.Provider.Settings.ActionApplicationDetailsSettings,
            //        Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
            //        intent.AddFlags(ActivityFlags.NewTask);

            //        Android.App.Application.Context.StartActivity(intent);
            //        break;
            //}
        }
    }
}