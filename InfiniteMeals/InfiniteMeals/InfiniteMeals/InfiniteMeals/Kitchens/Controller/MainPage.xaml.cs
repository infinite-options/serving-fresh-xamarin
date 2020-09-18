using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using InfiniteMeals.Kitchens.Model;
using System.Collections.ObjectModel;
using System.Globalization;
using InfiniteMeals.Refund;
using InfiniteMeals.Information;
using InfiniteMeals.PromptAddress;
using Plugin.LatestVersion;
using InfiniteMeals.Checkout;
using Xamarin.Essentials;
using Acr.UserDialogs;
using Newtonsoft.Json;
//using Foundation;
//using UIKit;
//using Android.Content;

namespace InfiniteMeals
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<KitchensModel> Kitchens = new ObservableCollection<KitchensModel>();

        // THIS LIST WILL CONTAIN ALL WEEKS OPENED AND CLOSED HOURS
        public IList<IList<string>> businessDailyHours = null;
        public IList<IList<string>> businessDailyAcceptingHours = null;
        public IList<IList<string>> businessDeliveryHours = null;

        // THIS CLASS CONTAINTS THE LIST OF HOURS FOR EVERY BUSINESS BY DAY
        public class BusinessHours
        {
            public IList<string> Friday { get; set; }
            public IList<string> Monday { get; set; }
            public IList<string> Sunday { get; set; }
            public IList<string> Tuesday { get; set; }
            public IList<string> Saturday { get; set; }
            public IList<string> Thursday { get; set; }
            public IList<string> Wednesday { get; set; }
        }

        public MainPage()
        {
            InitializeComponent();
            // CheckVersion();
            //try
            //{
            //    GetKitchens();
            //}
            //catch (TaskCanceledException ex)
            //{
            //    GetKitchens();
            //}
            //// Kitchens.Clear();

            //kitchensListView.RefreshCommand = new Command(() =>
            //{
            //    GetKitchens();
            //    kitchensListView.IsRefreshing = false;
            //});

            kitchensListView.ItemSelected += Handle_ItemTapped();

        }

        protected async void GetKitchens()
        {
            var request = new HttpRequestMessage();
            // request.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/kitchens");
            request.RequestUri = new Uri("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/businesses");
            request.Method = HttpMethod.Get;

            var client = new HttpClient();
            try
            {
                HttpResponseMessage response = await client.SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    ServingFreshBusiness business = new ServingFreshBusiness();
                    business = JsonConvert.DeserializeObject<ServingFreshBusiness>(data);

                    var currentDay = DateTime.Now.DayOfWeek.ToString();
                    var time = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                    this.Kitchens.Clear();

                    for(int i = 0; i < business.result.result.Count;i++)
                    {
                        // SETTING LIST OF DAILY BUSINESS HOURS - RDS
                        businessDailyHours = new List<IList<string>>();
                        businessDailyAcceptingHours = new List<IList<string>>();
                        businessDeliveryHours = new List<IList<string>>();

                        SetListOfBusinessHours(business.result.result[i].business_hours);
                        SetListOfBusinessAcceptingHours(business.result.result[i].business_accepting_hours);
                        SetListOfBusinessDeliveryHours(business.result.result[i].business_delivery_hours);

                        try
                        {
                            Console.WriteLine("available_zip:" + GetZipCodeListByBussinessName(business.result.result[i].business_name));
                        }
                        catch
                        {
                            Console.WriteLine("no");
                        }

                        Boolean businessIsOpen;
                        string accepting_hours;
                        int dayOfWeekIndex = getDayOfWeekIndex(DateTime.Today);

                        // CHECKING CURRENT BUSINESS IS OPERATING
                        if (IsOpened() == false)
                        {
                            accepting_hours = "Not accepting orders, no meals";
                            businessIsOpen = false;
                        }
                        else if (IsOpened24Hrs(dayOfWeekIndex) == true)
                        {
                            accepting_hours = "24 hours";
                            businessIsOpen = true;
                        }
                        else
                        {
                            Boolean isAccepting = IsBusinessAccepting(dayOfWeekIndex, DateTime.Parse(time));
                            businessIsOpen = IsBusinessOpen(TimeSpan.Parse(businessDailyAcceptingHours[dayOfWeekIndex][0]), TimeSpan.Parse(businessDailyAcceptingHours[dayOfWeekIndex][1]), isAccepting);
                            accepting_hours = WhenAccepting(businessIsOpen, dayOfWeekIndex);
                        }
                        
                        string delivery_hours = WhenDelivering(dayOfWeekIndex);
                        bool is_available = false;
                        string zipcodes = "";
                        try
                        {
                            string customer_zipcode = Application.Current.Properties["zip"].ToString();

                            zipcodes = GetZipCodeListByBussinessName(business.result.result[i].business_name);
                            
                            string[] zipcodes_array = zipcodes.Split(',');
                            foreach (var zipcode in zipcodes_array)
                            {
                                if (zipcode == customer_zipcode)
                                {
                                    is_available = true;
                                    break;
                                }
                            }
                            
                        }
                        catch
                        {
                            is_available = true;
                            zipcodes = "";
                        }

                        if (is_available)
                        {
                            this.Kitchens.Add(new KitchensModel()
                            {
                                kitchen_id = business.result.result[i].business_uid,
                                zipcode = business.result.result[i].business_zip,
                                title = business.result.result[i].business_name,
                                open_hours = accepting_hours,
                                delivery_period = delivery_hours,
                                description = business.result.result[i].business_desc,
                                isOpen = businessIsOpen,
                                status = (businessIsOpen == true) ? "Open now" : "Closed",
                                statusColor = (businessIsOpen == true) ? "Green" : "#a0050f",
                                opacity = (businessIsOpen == true) ? "1.0" : "0.6",
                                available_zipcode = zipcodes
                            }
                            );
                            if (business.result.result[i].business_desc != null)
                            {
                                Console.WriteLine("This is the length of the a desc" + business.result.result[i].business_desc.Length);
                            }

                        }
                        else
                        {
                            if (!zipcodes.Equals(""))
                            {

                                this.Kitchens.Add(new KitchensModel()
                                {
                                    kitchen_id = business.result.result[i].business_uid,
                                    zipcode = business.result.result[i].business_zip,
                                    title = business.result.result[i].business_name,
                                    open_hours = accepting_hours,
                                    delivery_period = delivery_hours,
                                    description = business.result.result[i].business_desc,
                                    isOpen = false,
                                    status = "Not available for your address",
                                    statusColor = "#a0050f",
                                    opacity = "0.6",
                                    available_zipcode = zipcodes
                                });
                            }
                        }
                    }
                    
                    kitchensListView.ItemsSource = Kitchens;

                }
            }
            catch (TaskCanceledException ex)
            {
                GetKitchens();
            }
        }

        // HELPER FUNCTION 1
        // THIS FUNCTION SETS UP THE A LIST OF ZIPCODES
        public string GetZipCodeListByBussinessName(string businessName)
        {
            if (businessName.Equals("Resendiz Family Fruit Barn"))
            {
                return "94024,94087,95014,95030,95032,95051,95070,95111,95112,95120,95123,95124,95125,95129,95130,95128,95122,95118,95126,95136,95113,95117,95050";
            }
            if (businessName.Equals("Esquivel Farm"))
            {
                return "94024,94087,95014,95030,95032,95051,95070,95111,95112,95120,95123,95124,95125,95129,95130,95128,95122,95118,95126,95136,95113,95117,95050";
            }
            if (businessName.Equals("Almaden Farmer's Market"))
            {
                return "94024,94087,95014,95030,95032,95051,95070,95111,95112,95120,95123,95124,95125,95129,95130,95128,95122,95118,95126,95136,95113,95117,95050";
            }
            if (businessName.Equals("Nitya Ayurveda"))
            {
                return "94024,94087,95014,95030,95032,95051,95070,95111,95112,95120,95123,95124,95125,95129,95130,95128,95122,95118,95126,95136,95113,95117,95050";
            }
            // WE DON'T HAVE THE LIST OF ZIPCODES FOR THE GIVEN BUSINESS
            return "";
        }

        // HELPER FUNCTION 2
        // THIS FUNCTION INSERTS THE BUSINESS HOURS IN A LIST EACH INDEX
        // REPRESENT A WEEKDAY
        private void SetListOfBusinessHours(string hoursStr)
        {
            BusinessHours hours = JsonConvert.DeserializeObject<BusinessHours>(hoursStr);

            businessDailyHours.Add(hours.Sunday);
            businessDailyHours.Add(hours.Monday);
            businessDailyHours.Add(hours.Tuesday);
            businessDailyHours.Add(hours.Wednesday);
            businessDailyHours.Add(hours.Thursday);
            businessDailyHours.Add(hours.Friday);
            businessDailyHours.Add(hours.Saturday);
        }

        // HELPER FUNCTION 3
        // THIS FUNCTION INSERTS THE BUSINESS HOURS IN A LIST EACH INDEX
        // REPRESENT A WEEKDAY
        private void SetListOfBusinessAcceptingHours(string hoursStr)
        {
            BusinessHours hours = JsonConvert.DeserializeObject<BusinessHours>(hoursStr);

            businessDailyAcceptingHours.Add(hours.Sunday);
            businessDailyAcceptingHours.Add(hours.Monday);
            businessDailyAcceptingHours.Add(hours.Tuesday);
            businessDailyAcceptingHours.Add(hours.Wednesday);
            businessDailyAcceptingHours.Add(hours.Thursday);
            businessDailyAcceptingHours.Add(hours.Friday);
            businessDailyAcceptingHours.Add(hours.Saturday);
        }

        // HELPER FUNCTION 4
        // THIS FUNCTION INSERTS THE BUSINESS HOURS IN A LIST EACH INDEX
        // REPRESENT A WEEKDAY
        private void SetListOfBusinessDeliveryHours(string hoursStr)
        {
            BusinessHours hours = JsonConvert.DeserializeObject<BusinessHours>(hoursStr);

            businessDeliveryHours.Add(hours.Sunday);
            businessDeliveryHours.Add(hours.Monday);
            businessDeliveryHours.Add(hours.Tuesday);
            businessDeliveryHours.Add(hours.Wednesday);
            businessDeliveryHours.Add(hours.Thursday);
            businessDeliveryHours.Add(hours.Friday);
            businessDeliveryHours.Add(hours.Saturday);
        }

        // HELPER FUNCTION 5
        // THIS FUNCTION DETERMINES IF A BUSINESS IS OPERATING ANY DAY
        // IF AT LEAST ONE DAY IS OPEN, THEN SAY IT IS OPERATING. OTHERWISE IS NOT.
        private bool IsOpened()
        {
            int openedTime = 0;
            int closedTime = 1;

            for(int i = 0; i < businessDailyHours.Count; i++)
            {
                DateTime opened = DateTime.Parse(businessDailyHours[i][openedTime]);
                DateTime closed = DateTime.Parse(businessDailyHours[i][closedTime]);

                if (DateTime.Compare(opened, closed) != 0)
                {
                    return true;
                }
            }
            return false;
        }

        // HELPER FUNCTION 6
        // THIS FUNCTION DETERMINES IF A BUSINEES IS OPERATING 24HOURS
        private bool IsOpened24Hrs(int currentDayIndex)
        {
            var opened = businessDailyHours[currentDayIndex][0];
            var closed = businessDailyHours[currentDayIndex][1];

            if(opened.Equals("00:00:00") && closed.Equals("23:59:59")){
                return true;
            }
            return false;
        }

        // HELPER FUNCTION 7
        // THIS FUNCTION DETERMINE IF THE CURRENT BUSINESS ARE ACCEPTING ORDERS
        // BASED ON CURRENT DAY AND TIME
        private bool IsBusinessAccepting(int currentDayIndex, DateTime currentTime)
        {
            int beforeClosedTime = -1;
            int inTime = 0;
            int afterOpenedTime = 1;

            int openedTime = 0;
            int closedTime = 1;

            DateTime opened = DateTime.Parse(businessDailyAcceptingHours[currentDayIndex][openedTime]);
            DateTime closed = DateTime.Parse(businessDailyAcceptingHours[currentDayIndex][closedTime]);

            // BUSINESS IS NOT ACCEPTING ORDERS TODAY
            if (DateTime.Compare(opened, closed) == 0)
            {
                return false;
            }
            else
            {
                // BUSINESS ACCEPT ORDERS IF WITH IN THE ACCEPTING HOURS RANGE
                if (DateTime.Compare(currentTime, opened) == inTime
                    || DateTime.Compare(currentTime, opened) == afterOpenedTime && DateTime.Compare(currentTime, closed) == beforeClosedTime
                    || DateTime.Compare(currentTime, closed) == inTime)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        // HELPER FUNCTION 8
        // THIS FUNCTION DETERMINES WHEN IS THE NEXT ACCEPTING TIME AVAILABLE DAY DETERMINE BY THE INDEX OF THE LIST
        // NOTE: IF OPENED == CLOSED TIME RETURN FALSE;
        private bool NextAcceptingDay(int dayIndex, int orderOrDelivery)
        {
            // 0 = Order
            DateTime opened = new DateTime();
            DateTime closed = new DateTime();
            if (orderOrDelivery == 0)
            {
                opened = DateTime.Parse(businessDailyAcceptingHours[dayIndex][0]);
                closed = DateTime.Parse(businessDailyAcceptingHours[dayIndex][1]);
            }
            else
            {
                opened = DateTime.Parse(businessDeliveryHours[dayIndex][0]);
                closed = DateTime.Parse(businessDeliveryHours[dayIndex][1]);
            }


            if (DateTime.Compare(opened, closed) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async void CheckVersion()
        {
            string latestVersionNumber = await CrossLatestVersion.Current.GetLatestVersionNumber();
            string installedVersionNumber = CrossLatestVersion.Current.InstalledVersionNumber;
            var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();

            if (!isLatest)
            {
                bool update = await DisplayAlert("Serving Now\nhas gotten even better!", "Please visit the App Store to get the latest version.", "Yes", "No"); ;

                if (update)
                {
                    await CrossLatestVersion.Current.OpenAppInStore();
                }
            }
        }

        private EventHandler<SelectedItemChangedEventArgs> Handle_ItemTapped()
        {
            return OnItemSelected;
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // disable selected item highlighting;
            if (e.SelectedItem == null) return;
            ((ListView)sender).SelectedItem = null;


            // do something with the selection
            var kitchen = e.SelectedItem as KitchensModel;

            // disable selection if the kitchen is closed
            if (kitchen.isOpen == false)
            {
                return;
            }

            var guid = Preferences.Get("guid", null);
            if (guid == null)
            {
                var will_enable = await DisplayAlert("Serving Now works better with Notifications Enabled", "Please consider turning on Notifications so that we can Confirm your order was received and Notify you when your order has been delivered", "No Thanks", "Yes! Send Notifications");
                if (!will_enable)
                {
                    await DisplayAlert("Thank you!", "Make sure you restart the app after you've enabled", "Go to app setting");
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
                    //        //Intent intent = new Intent(
                    //        //Android.Provider.Settings.ActionApplicationDetailsSettings,
                    //        //Android.Net.Uri.Parse("package:" + Android.App.Application.Context.PackageName));
                    //        //intent.AddFlags(ActivityFlags.NewTask);

                    //        //Android.App.Application.Context.StartActivity(intent);
                    //        break;
                    //}
                    DependencyService.Get<ISettingsHelper>().OpenAppSettings();
                }
            }
             await Navigation.PushAsync(new SelectMealOptions(kitchen.kitchen_id, kitchen.title, kitchen.zipcode, kitchen.available_zipcode));
        }

        //  Get integer index of day of the week, with 0 as Sunday
        private int getDayOfWeekIndex(DateTime day)
        {
            if (day.DayOfWeek == DayOfWeek.Sunday)
                return 0;
            if (day.DayOfWeek == DayOfWeek.Monday)
                return 1;
            if (day.DayOfWeek == DayOfWeek.Tuesday)
                return 2;
            if (day.DayOfWeek == DayOfWeek.Wednesday)
                return 3;
            if (day.DayOfWeek == DayOfWeek.Thursday)
                return 4;
            if (day.DayOfWeek == DayOfWeek.Friday)
                return 5;
            if (day.DayOfWeek == DayOfWeek.Saturday)
                return 6;
            return -1;
        }

        //  Get day of week string from intger index of the week, with 0 as Sunday
        private string getDayOfWeekFromIndex(int index)
        {
            if (index == 0)
                return "Sunday";
            if (index == 1)
                return "Monday";
            if (index == 2)
                return "Tuesday";
            if (index == 3)
                return "Wednesday";
            if (index == 4)
                return "Thursday";
            if (index == 5)
                return "Friday";
            if (index == 6)
                return "Saturday";
            return "Error";
        }

        //  Is the store already closed or is it opening later today?
        private int isAlreadyClosed(DateTime endTime)
        {
            string currentTime = DateTime.Now.TimeOfDay.ToString().Substring(0, 8);
                
            if (DateTime.Compare(DateTime.Parse(currentTime), endTime) < 0)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }

        //  When is the business accepting orders?
        private string WhenAccepting(Boolean businessIsOpen, int dayOfWeekIndex)
        {
            //JArray converted_accepting_hours = JArray.Parse((string)k["accepting_hours"]);
            //string end_time_12 = ConvertFromToTime(converted_accepting_hours[dayOfWeekIndex]["M"]["close_time"]["S"].ToString(), "HH:mm", "h:mm tt");

            string end_time_24 = businessDailyAcceptingHours[dayOfWeekIndex][1];

            if (businessIsOpen == true)
            {
               
                return "Until " + ConvertFromToTime(end_time_24.Substring(0, 5), "HH:mm", "h:mm tt");
            }
            int nextOpenDay = nextPeriodDayIndex(dayOfWeekIndex, isAlreadyClosed(DateTime.Parse(end_time_24)), 0);
            if (nextOpenDay == -1)
            {
                return "Not currently accepting orders";
            }
            string next_day;
            if (nextOpenDay == dayOfWeekIndex)
            {
                next_day = "today";
            }
            else if (nextOpenDay == (dayOfWeekIndex + 1) % 7)
            {
                next_day = "tomorrow";
            }
            else
            {
                next_day = getDayOfWeekFromIndex(nextOpenDay);
            }
            // INDEX 1 MEANS CLOSED TIME 
            return "Starting " + next_day + " " + ConvertFromToTime(businessDailyAcceptingHours[nextOpenDay][1].Substring(0, 5), "HH:mm", "h:mm tt");
        }

        //  When is the next delivery period?
        private string WhenDelivering(int dayOfWeekIndex)
        {
           // JArray converted_accepting_hours = JArray.Parse((string)k["accepting_hours"]);
            int nextDeliveryDayIndex = nextPeriodDayIndex(dayOfWeekIndex, 1, 1);
            
            if (nextDeliveryDayIndex != -1)
            {
                var deliveryOpenTime = businessDeliveryHours[nextDeliveryDayIndex][0];
                var deliveryCloseTime = businessDeliveryHours[nextDeliveryDayIndex][1];
                return getDayOfWeekFromIndex(nextDeliveryDayIndex) + " " + ConvertFromToTime(deliveryOpenTime.Substring(0,5), "HH:mm", "h:mm tt") + " - " + ConvertFromToTime(deliveryCloseTime.Substring(0, 5), "HH:mm", "h:mm tt");
            }
            else
            {
                return "Not currently delivering";
            }
        }

        //  Get integer index of day of the week of next time (accepting or delivering) period, with 0 as Sunday
        //  3rd and 4th arguments take kitchen API list and boolean keys (e.g. delivery_hours and is_delivering)
        //  5th argument takes the number of days to wait before beginning the check. For instance, if you want to find the next delivery period starting the day after today, argument should be 1.
        private int nextPeriodDayIndex(int dayOfTheWeek, int dayDelay, int orderOrDelivery)
        {
            //JArray converted_accepting_hours = JArray.Parse((string)kitchen[list_key]);

            int dayIndex;
            for (int i = dayDelay; i < dayDelay + 7; i++)
            {
                dayIndex = (dayOfTheWeek + i) % 7;
                if (NextAcceptingDay(dayIndex, orderOrDelivery) == true)
                {
                    return dayIndex;
                }
            }
            return -1;
        }

        //  Function checking if business is currently open
        private Boolean IsBusinessOpen(TimeSpan open_time, TimeSpan close_time, Boolean is_accepting)
        {
            TimeSpan now = DateTime.Now.TimeOfDay;
            //  Accepting orders on current day?
            if (is_accepting == false)
            {
                return false;
            }
            else
            {
                //  Opening and closing hours on same day
                if (open_time <= close_time)
                {
                    if (now >= open_time && now <= close_time)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                //  Opening and closing hours on different day
                else
                {
                    if (now >= open_time || now <= close_time)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        private string formatZipcode(string zipcode)
        {
            if (zipcode == "95120")
            {
                return "Almaden";
            }
            if (zipcode == "95135")
            {
                return "Evergreen";
            }
            if (zipcode == "95060")
            {
                return "Santa Cruz";
            }
            return "Other";
        }

        // https://www.niceonecode.com/Question/20540/how-to-convert-24-hours-string-format-to-12-hours-format-in-csharp
        public static string ConvertFromToTime(string timeHour, string inputFormat, string outputFormat)
        {
            var timeFromInput = DateTime.ParseExact(timeHour, inputFormat, null, DateTimeStyles.None);
            string timeOutput = timeFromInput.ToString(
                outputFormat,
                CultureInfo.InvariantCulture);
            return timeOutput;
        }

        void NavigateToRefund(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ProductRefundPage());
        }
        void NavigateToInformation(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InformationPage());
            //Navigation.PushAsync(new SocialLoginPage());
        }
        void PromptForAddress(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PromptAddressPage());
        }
    }
}
