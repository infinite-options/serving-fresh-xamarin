using Android.Content;
using InfiniteMeals.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(SettingsHelper))]
namespace InfiniteMeals.Droid
{
    public class SettingsHelper : ISettingsHelper
    {
        public void OpenAppSettings()
        {
            Intent intent = new Intent(
            Android.Provider.Settings.ActionApplicationDetailsSettings,
            Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
            intent.AddFlags(ActivityFlags.NewTask);

            Android.App.Application.Context.StartActivity(intent);
        }
    }
}