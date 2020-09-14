using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using InfiniteMeals.Meals.Model;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;
using InfiniteMeals.Information;
using Plugin.LatestVersion;
using InfiniteMeals.PromptAddress;
using Newtonsoft.Json;

namespace InfiniteMeals
{

    public partial class SelectMealOptions : ContentPage
    {
        private int mealOrdersCount = 0;

        public ObservableCollection<MealsModel> Meals = new ObservableCollection<MealsModel>();

        private string kitchenID;
        private string kitchenZipcode;
        private string availableZipcode;

        public SelectMealOptions(string kitchen_id, string kitchen_name, string zipcode, string available_zipcode)
        {
            InitializeComponent();

            SetBinding(TitleProperty, new Binding(kitchen_name));

            kitchenID = kitchen_id;
            kitchenZipcode = zipcode;
            availableZipcode = available_zipcode;

            Console.WriteLine("kitchen_id = " + kitchen_id);
            Console.WriteLine("kitchen_name = " + kitchen_name);
            Console.WriteLine("zipcode = " + zipcode);
            Console.WriteLine("avaiable_zipcode" + available_zipcode);


            GetMeals(kitchenID);

            mealsListView.RefreshCommand = new Command(() =>
            {
                GetMeals(kitchenID);
                mealsListView.IsRefreshing = false;
            });
        }

        // GET MEALS FUNCTION UPDATED BY CARLOS
        protected async Task GetMeals(string kitchen_id)
        {
            // Console.WriteLine("kitchen_name: " + kitchen_name);
            NoMealsLabel.IsVisible = false;
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + kitchen_id);
            request.Method = HttpMethod.Get;

            var client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var mealsString = await content.ReadAsStringAsync();
                JObject meals = JObject.Parse(mealsString);
                String todaysDate = DateTime.Now.ToString("MM/dd/yyyy");
                Console.WriteLine("This is what you see in meals" + meals);

                string items = await response.Content.ReadAsStringAsync();
                ServingFreshBusinessItems data = new ServingFreshBusinessItems();

                data = JsonConvert.DeserializeObject<ServingFreshBusinessItems>(items);

                if (todaysDate[0] == '0')
                {
                    todaysDate = todaysDate.Substring(1);
                }
                todaysDate = todaysDate.Replace("/0", "/");

                this.Meals.Clear();

                NoMealsLabel.IsVisible = false;

                for(int i = 0; i < data.result.result.Count; i++)
                {
                    
                    NoMealsLabel.IsVisible = false;
                    this.Meals.Add(new MealsModel()
                    {
                        imageString = data.result.result[i].item_photo,
                        title = data.result.result[i].item_name,
                        price = data.result.result[i].item_price.ToString(),
                        description = "description",
                        kitchen_id = data.result.result[i].itm_business_uid,
                        id = data.result.result[i].item_uid,
                        qty = 0
                    }
                    );
                    
                }
                mealsListView.ItemsSource = Meals;
            }

        }

        // FUNCTION UPDATED BY CARLOS
        // THE VERSION ELSE STATEMENT WAS REMOVE FOR TESTING PURPOSES
        async void Handle_Clicked(object sender, System.EventArgs e)
        {
            //var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
            if (mealOrdersCount == 0)
            {
                await DisplayAlert("Order Error", "Please make an order to continue", "Continue");
            }
            //else if (!isLatest)
            //{
            //    await DisplayAlert("Update Required", "Please download the latest version of Serving Now from the app store", "Ok");

            //    await CrossLatestVersion.Current.OpenAppInStore();

            //}
            //else
            //{
                var secondPage = new CheckOutPage(Meals, kitchenID, kitchenZipcode, availableZipcode);
                await Navigation.PushAsync(secondPage);
            //}

            //var checkoutPage = new CheckOutPage();
            //await Navigation.PushAsync(checkoutPage);
        }

        private void reduceOrders(object sender, System.EventArgs e)
        {
            var button = (ImageButton)sender;
            var mealObject = (MealsModel)button.CommandParameter;

            if (mealObject != null)
            {
                if (mealObject.qty > 0)
                {
                    mealObject.qty -= 1;
                    mealOrdersCount -= 1;
                }
            }
        }

        private void addOrders(object sender, System.EventArgs e)
        {
        
            var button = (ImageButton)sender;
            var mealObject = (MealsModel)button.CommandParameter;

            if (mealObject != null)
            {
                if (mealObject.qty < 50)
                {
                    mealObject.qty += 1;
                    mealOrdersCount += 1;
                }
            }
        }

        void NavigateToInformation(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InformationPage());
        }

        void PromptForAddress(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PromptAddressPage());
        }

        //public string createUrl()
        //{
        //    DateTime dateTime = DateTime.UtcNow.Date;
        //    var today = dateTime.ToString("yyyyMMdd");

        //    string url = "https://s3-us-west-2.amazonaws.com/ordermealapp/" + today;
        //    return url;
        //}
    }

}
