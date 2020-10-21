using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Json;
using Coupon = InfiniteMeals.Checkout.Coupon;

//using Stripe;


namespace InfiniteMeals
{ 

    public partial class profileUser : ContentPage
    {
        static bool state = false;

        // WeeklyPurchaseCoupon(string userEmail)
        // Determines if user needs a weekly purchase coupon awarded
        // Returns 1 if coupon was awarded, 0 if it was not, -1 if user already has coupon
        //async public void WeeklyPurchaseCoupon(string userEmail)
        //{
        //    string historyEndpoint = "https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/history/";
        //    string userEndpoint = String.Concat(historyEndpoint, userEmail);
        //    // Open Http Connection
        //    var client = new HttpClient();
        //    // Get Data from endpoint
        //    var RDSresponse = await client.GetAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/history/abc@gmail.com");
        //    var message = await RDSresponse.Content.ReadAsStringAsync();
        //    System.Diagnostics.Debug.WriteLine(RDSresponse.IsSuccessStatusCode);
        //    JObject history = JObject.Parse(message);               // Parse Object to get as JObject
        //    JArray result = history["result"].Value<JArray>();      // Get useful info from Object
        //    System.Diagnostics.Debug.WriteLine(result);
        //    List<DateTime> orderDates = new List<DateTime>();
        //    if (result.Count >= 5)                                  // five orders exists
        //    {
        //        foreach (JObject item in result.Children())         // put last 5 order dates into list
        //        {
        //            string sDate = item["purchase_date"].Value<string>();
        //            DateTime date = DateTime.Parse(sDate);
        //            orderDates.Add(date);
        //        }
        //        System.Diagnostics.Debug.WriteLine(String.Join("\n", orderDates));
        //        string giveCoupon = "true";
        //        for (int i = 0; i < orderDates.Count - 2; i++)                  // look at pairs 0-1, 1-2, 2-3, 3-4
        //        {
        //            TimeSpan interval = orderDates[i + 1] - orderDates[i];      // get interval between orders
        //            if (interval.Days > 7)                                       // any interval > 7 days
        //            {
        //                System.Diagnostics.Debug.WriteLine("Coupon was not earned");
        //               giveCoupon = "false";                                     // This coupon is not awarded
        //            }
        //        }

        //        Application.Current.Properties["weeklyPurchaseCoupon"] = giveCoupon;
        //        // Post coupon to Coupons Endpoint

        //    } 
        //}

        public profileUser()
        {
            InitializeComponent();

            if (Application.Current.Properties.ContainsKey("userEmailAddress"))     // Check userEmailAddress in dict
            {
                userEmailAddress.Text = (string)Application.Current.Properties["userEmailAddress"];
            }
            if (Application.Current.Properties.ContainsKey("userFirstName"))        // Check userFirstName in dict
            {
                userFirstName.Text = (string)Application.Current.Properties["userFirstName"];
            }
            if (Application.Current.Properties.ContainsKey("userLastName"))         // Check userLastName in dict
            {
                userLastName.Text = (string)Application.Current.Properties["userLastName"];
            }
            userPassword.Text = "*******";
            userConfirmPassword.Text = "*******";
            if (Application.Current.Properties.ContainsKey("userAddress"))          // Check userAddress in dict
            {
                userAddress.Text = (string)Application.Current.Properties["userAddress"];
            }
            if (Application.Current.Properties.ContainsKey("userAddressUnit"))      // Check userAddressUnit in dict
            {
                userUnitNumber.Text = (string)Application.Current.Properties["userAddressUnit"];
            }
            if (Application.Current.Properties.ContainsKey("userCity"))             // Check userCity in dict
            {
                userCity.Text = (string)Application.Current.Properties["userCity"];
            }
            if (Application.Current.Properties.ContainsKey("userState"))            // Check userState in dict
            {
                userState.Text = (string)Application.Current.Properties["userState"];
            }
            if (Application.Current.Properties.ContainsKey("userZipCode"))          // Check userZipCode in dict
            {
                userZipcode.Text = (string)Application.Current.Properties["userZipCode"];
            }
            if (Application.Current.Properties.ContainsKey("userPhoneNumber"))      // Check userPhoneNumber in dict
            {
                userPhoneNumber.Text = (string)Application.Current.Properties["userPhoneNumber"];
            }
            if (Application.Current.Properties.ContainsKey("latitude"))            // Check latitude in dict (assume longitude exists)
            {
                // only do map actions if a location can be found
                Position position = new Position(Double.Parse(Application.Current.Properties["latitude"].ToString()), Double.Parse(Application.Current.Properties["longitude"].ToString()));
                map.MapType = MapType.Street;
                var mapSpan = new MapSpan(position, 0.001, 0.001);

                Pin address = new Pin();
                address.Label = "Delivery Address";
                address.Type = PinType.SearchResult;
                address.Position = position;

                map.MoveToRegion(mapSpan);
                map.Pins.Add(address);
            }
            
        }

        //void Button_Clicked(System.Object sender, System.EventArgs e)
        //{
        //    string password = "infiniteOptions";
        //    string startPassword = "*************";
        //    if (!state)
        //    {
        //        passwordUser.Text = password;
        //        state = true;
        //    }
        //    else
        //    {
        //        passwordUser.Text = startPassword;
        //        state = false;
        //    }
        //}

        //void Button_Clicked_1(System.Object sender, System.EventArgs e)
        //{
        //    string password = "infiniteOptions";
        //    string startPassword = "*************";
        //    if (!state)
        //    {
        //        confirmPassowordUser.Text = password;
        //        state = true;
        //    }
        //    else
        //    {
        //        confirmPassowordUser.Text = startPassword;
        //        state = false;
        //    }
        //}

        void ImageButton_Clicked(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.CheckoutPage();
        }

        void InfoClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new InfoPage();
        }

        void ProfileClick(System.Object sender, System.EventArgs e)
        {
            // AGAIN SINCE YOU ARE IN THE PROFILE PAGE NOTHING SHOULD HAPPEN
            // WHEN CLICK
        }

        void SaveChangesClick(System.Object sender, System.EventArgs e)
        {
            
        }

        private async void RewardsClicked(object sender, EventArgs e)
        {
            // set top buttons to white and orange
            DeliveryInfoButton.TextColor = Color.White;
            RewardsButton.TextColor = Color.FromHex("#FF8500");

            // Disable all info for reward view
            userAddress.IsVisible = false;
            userCity.IsVisible = false;
            userConfirmPassword.IsVisible = false;
            userEmailAddress.IsVisible = false;
            userFirstName.IsVisible = false;
            userLastName.IsVisible = false;
            userPassword.IsVisible = false;
            userPhoneNumber.IsVisible = false;
            userState.IsVisible = false;
            userUnitNumber.IsVisible = false;
            userZipcode.IsVisible = false;
            map.IsVisible = false;
            pushNotificationEntry.IsVisible = false;
            pushNotificationSwitch.IsVisible = false;
            mapFrame.IsVisible = false;
            saveChangesButton.IsVisible = false;

            // set Rewards items to visible
            reward1.IsVisible = true;
            reward2.IsVisible = true;
            reward3.IsVisible = true;
            // Use Endpoint to get user data:
            if (Application.Current.Properties.ContainsKey("userEmailAddress"))
            {
                string userEmailAddress = Application.Current.Properties["userEmailAddress"].ToString();
                bool weeklyCoupon = Coupon.WeeklyOrderCoupon(userEmailAddress).Result;
                if (weeklyCoupon)
                {
                    reward1.Text = "Coupon Available as a reward for weekly purchases";
                }
                else
                {
                    reward1.Text = "Weekly Purchase Coupon Unavalable";
                }
            }



            //reward1.Text = String.Format("{0} / 9 orders completed for reward 1", orders);
            //reward1PNG.Source = String.Format("Reward{0}.png", orders);
        }

        private void DeliveryInfoButton_Clicked(object sender, EventArgs e)
        {
            // set top buttons to white and orange
            RewardsButton.TextColor = Color.White;
            DeliveryInfoButton.TextColor = Color.FromHex("#FF8500");

            // Disable all info for reward view
            userAddress.IsVisible = true;
            userCity.IsVisible = true;
            userConfirmPassword.IsVisible = true;
            userEmailAddress.IsVisible = true;
            userFirstName.IsVisible = true;
            userLastName.IsVisible = true;
            userPassword.IsVisible = true;
            userPhoneNumber.IsVisible = true;
            userState.IsVisible = true;
            userUnitNumber.IsVisible = true;
            userZipcode.IsVisible = true;
            map.IsVisible = true;
            pushNotificationEntry.IsVisible = true;
            pushNotificationSwitch.IsVisible = true;
            mapFrame.IsVisible = true;
            saveChangesButton.IsVisible = true;

            // set Rewards items to visible
            reward1.IsVisible = false;
            reward2.IsVisible = false;
            reward3.IsVisible = false;
        }
    }
}
