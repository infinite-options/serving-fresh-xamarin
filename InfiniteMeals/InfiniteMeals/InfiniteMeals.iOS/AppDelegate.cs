using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using Plugin.GoogleClient;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using InfiniteMeals.Models;

namespace InfiniteMeals.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub Hub { get; set; }
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        //public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        //{
        //    //Xamarin.Calabash.Start();
        //    global::Xamarin.Forms.Forms.Init();

        //    LoadApplication(new App());

        //    return base.FinishedLaunching(app, options);
        //}
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {

            Debug.WriteLine("FinishedLaunching function");
            global::Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            Debug.WriteLine("Finished Init()");
            //GoogleClientManager.Initialize();
            LoadApplication(new App());
            Debug.WriteLine("Finished LoadApp()");
            //Task.Delay(1500).Wait();
            base.FinishedLaunching(app, options);
            Debug.WriteLine("Finished base.FinishedLaunching");
            RegisterForRemoteNotifications();
            Debug.WriteLine("Finished RegisterForRemoteNotification()");

            UINavigationBar.Appearance.TintColor = Color.FromHex("#FFFFFF").ToUIColor();

            //UIView statusBar = UIApplication.SharedApplication.ValueForKey(new NSString("statusBar")) as UIView;
            //if (statusBar != null && statusBar.RespondsToSelector(new ObjCRuntime.Selector("setBackgroundColor:")))
            //{
            //    // change to your desired color 
            //    statusBar.BackgroundColor = Color.FromHex("#7f6550").ToUIColor();
            //}

            // Color of the selected tab icon:
            UITabBar.Appearance.SelectedImageTintColor = Constants.SecondaryColor.ToUIColor();

            // Color of the tabbar background:
            //UITabBar.Appearance.BarTintColor = UIColor.FromRGB(247, 247, 247);

            // Color of the selected tab text color:
            UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    TextColor = Constants.SecondaryColor.ToUIColor()
        },
                UIControlState.Selected);

            // Color of the unselected tab icon & text:
            UITabBarItem.Appearance.SetTitleTextAttributes(
                new UITextAttributes()
                {
                    TextColor = Color.FromHex("#000000").ToUIColor()
                },
                UIControlState.Normal);
            UIView.AppearanceWhenContainedIn(typeof(UIAlertView)).TintColor = Constants.SecondaryColor.ToUIColor();
            UIView.AppearanceWhenContainedIn(typeof(UIAlertController)).TintColor = Constants.SecondaryColor.ToUIColor();
            return true;
        }
        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            return GoogleClientManager.OnOpenUrl(app, url, options);
        }
        void RegisterForRemoteNotifications()
        {
            // register for remote notifications based on system version
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                Debug.WriteLine("1st if");
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert |
                    UNAuthorizationOptions.Sound |
                    UNAuthorizationOptions.Sound,
                    (granted, error) =>
                    {
                        if (granted)
                            InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                    });
            }
            else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var pushSettings = UIUserNotificationSettings.GetSettingsForTypes(
                UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound,
                new NSSet());

                UIApplication.SharedApplication.RegisterUserNotificationSettings(pushSettings);
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            Debug.WriteLine("Registered");
            if (Preferences.Get("guid", null) != null)
            {
                return;
            }

            Hub = new SBNotificationHub(AppConstants.ListenConnectionString, AppConstants.NotificationHubName);

            // update registration with Azure Notification Hub
            Hub.UnregisterAll(deviceToken, (error) =>
            {
                if (error != null)
                {
                    Debug.WriteLine($"Unable to call unregister {error}");
                    return;
                }

                var guid = Guid.NewGuid();
                var tag = "guid_" + guid.ToString();
                Console.WriteLine("guid:" + tag);
                Preferences.Set("guid", tag);

                var tags = new NSSet(AppConstants.SubscriptionTags.Append(tag).ToArray());
                
                Hub.RegisterNative(deviceToken, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        Debug.WriteLine($"RegisterNativeAsync error: {errorCallback}");
                    }
                });

                var templateExpiration = DateTime.Now.AddDays(120).ToString(System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
                Hub.RegisterTemplate(deviceToken, "defaultTemplate", AppConstants.APNTemplateBody, templateExpiration, tags, (errorCallback) =>
                {
                    if (errorCallback != null)
                    {
                        if (errorCallback != null)
                        {
                            Debug.WriteLine($"RegisterTemplateAsync error: {errorCallback}");
                        }
                    }
                });
            });
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            ProcessNotification(userInfo, false);
        }

        void ProcessNotification(NSDictionary options, bool fromFinishedLaunching)
        {
            // make sure we have a payload
            if (options != null && options.ContainsKey(new NSString("aps")))
            {
                // get the APS dictionary and extract message payload. Message JSON will be converted
                // into a NSDictionary so more complex payloads may require more processing
                NSDictionary aps = options.ObjectForKey(new NSString("aps")) as NSDictionary;
                string payload = string.Empty;
                NSString payloadKey = new NSString("alert");
                if (aps.ContainsKey(payloadKey))
                {
                    payload = aps[payloadKey].ToString();
                }

                if (!string.IsNullOrWhiteSpace(payload))
                {
                    //(App.Current.MainPage as MainPage)?.AddMessage(payload);
                    Console.WriteLine(payload);
                }

            }
            else
            {
                Debug.WriteLine($"Received request to process notification but there was no payload.");
            }
        }
    }
}
