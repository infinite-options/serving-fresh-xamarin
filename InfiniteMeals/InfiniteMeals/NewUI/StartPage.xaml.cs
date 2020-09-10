using System;
using System.Collections.Generic;

using Xamarin.Forms;

using InfiniteMeals.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace InfiniteMeals.NewUI
{
    public partial class StartPage : ContentPage
    {
        ObservableCollection<DeliveriesModel> Deliveries = new ObservableCollection<DeliveriesModel>();

        public StartPage()
        {
            InitializeComponent();
            Init();
            GetDeliveries();
        }

        void Init()
        {
            BackgroundColor = Constants.PrimaryColor;
        }

        async void GetDeliveries()
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/businesses");
            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);
            string data = await response.Content.ReadAsStringAsync();
            ServingFreshBusiness jsonResponse = new ServingFreshBusiness();
            jsonResponse = JsonConvert.DeserializeObject<ServingFreshBusiness>(data);
            Dictionary<string, List<FarmsModel>> farmsByDay = new Dictionary<string, List<FarmsModel>>();
            var days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            foreach (string day in days)
            {
                farmsByDay.Add(day, new List<FarmsModel>());
            }
            foreach (Business business in jsonResponse.result.result)
            {
                Dictionary<string, List<string>> delivery_hours = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(business.business_delivery_hours);
                foreach (KeyValuePair<string, List<string>> kvp in delivery_hours)
                {
                    if (kvp.Value[0] != kvp.Value[1])
                    {
                        farmsByDay[kvp.Key].Add(new FarmsModel()
                        {
                            name = business.business_name,
                            uid = business.business_uid
                        });
                    }
                }
            }
            var date = 19;
            foreach (string day in days)
            {
                Deliveries.Add(new DeliveriesModel()
                {
                    delivery_dayofweek = day,
                    delivery_date = "Sept "+date,
                    farms = farmsByDay[day]
                });
                date++;
            }
            delivery_list.ItemsSource = Deliveries;
        }

        void Button_Clicked(Object sender, EventArgs e)
        {
            DisplayAlert("Alert", "Button Pressed", "OK");
        }

        async void Open_Checkout(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CheckoutPage());
        }

        async void Open_Farm(Object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FarmPage());
        }

        void Change_Color(Object sender, EventArgs e)
        {
            if (sender is ImageButton imgbtn)
            {
                if (imgbtn.Effects[0] is TintImageEffect tint)
                {
                    imgbtn.Effects.RemoveAt(0);
                    if (tint.TintColor.Equals(Color.White))
                    {
                        tint.TintColor = Constants.SecondaryColor;
                    }
                    else
                    {
                        tint.TintColor = Color.White;
                    }
                    imgbtn.Effects.Insert(0, tint);
                }
            }
        }
    }
}
