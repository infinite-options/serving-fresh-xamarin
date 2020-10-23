
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;

namespace InfiniteMeals.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
		new[] { Intent.ActionView },
		Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
		DataSchemes = new[] { "com.infiniteoptions.socialloginsxamarin" },
		DataPath = "/oauth2redirect")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			//global::Android.Net.Uri uri_android = Intent.Data;

			//Uri uri_netfx = new Uri(uri_android.ToString());

			//         //new Task(() =>
			//         //{
			//         //    StartActivity(new Intent(Application.Context, typeof(MainActivity)));
			//         //}).Start();

			////load redirect_url Page
			//Console.WriteLine("This is link: " + uri_netfx);
			//         AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			//         //var intent = new Intent(this, typeof(MainActivity));
			//         //intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
			//         //StartActivity(intent);

			////var intent = new Intent(this, typeof(MainActivity)).SetFlags(ActivityFlags.ReorderToFront);
			////StartActivity(intent);
			///
		
			global::Android.Net.Uri uri_android = Intent.Data;

			Uri uri_netfx = new Uri(uri_android.ToString());

			// load redirect_url Page
			Console.WriteLine("This is the uri" + uri_netfx);
			AuthenticationState.Authenticator.OnPageLoading(uri_netfx);

			var intent = new Intent(this, typeof(MainActivity));
			intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);
			StartActivity(intent);

			this.Finish();

			return;
		}
	}
}
