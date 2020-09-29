using System;
using Foundation;
using InfiniteMeals.iOS;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsHelper))]
namespace InfiniteMeals.iOS
{
    public class SettingsHelper : ISettingsHelper
    {
        public void OpenAppSettings()
        {
            var url = new NSUrl($"app-settings:notifications");
            UIApplication.SharedApplication.OpenUrl(url);
        }
    }
}

