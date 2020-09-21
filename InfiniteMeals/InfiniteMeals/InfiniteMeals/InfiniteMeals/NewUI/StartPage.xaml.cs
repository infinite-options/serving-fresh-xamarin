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
        ObservableCollection<ItemsModel> datagrid = new ObservableCollection<ItemsModel>();
        ServingFreshBusinessItems data = new ServingFreshBusinessItems();

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

        async void Open_Checkout(Object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CheckoutPage());
        }

        void Open_Farm(Object sender, EventArgs e)
        {
            GetData(GetWeekDay(sender));
            Application.Current.MainPage = new businessItems(datagrid, GetWeekDay(sender));
        }

        async void GetData(string weekDay)
        {
            // THE "200-000004" WOULD GET REPLACE BY THE "weekDay" STRING
            var client = new HttpClient();
            var response = await client.GetAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + "200-000004");
            var d = await client.GetStringAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + "200-000004");
            if (response.IsSuccessStatusCode)
            {
                data = JsonConvert.DeserializeObject<ServingFreshBusinessItems>(d);

                // COMMENT THE FOLLOWING LINE OF CODE AS (CHANGE 2)
                // datagrid = new List<ItemModel>();
                this.datagrid.Clear();
                int n = data.result.result.Count;
                int j = 0;
                if (n == 0)
                {
                    this.datagrid.Add(new ItemsModel()
                    {
                        height = this.Width / 2 + 25,
                        width = this.Width / 2 - 25,
                        imageSourceLeft = "",
                        quantityLeft = 0,
                        itemNameLeft = "",
                        itemPriceLeft = "$ " + "",
                        isItemLeftVisiable = false,
                        isItemLeftEnable = false,
                        quantityL = 0,

                        imageSourceRight = "",
                        quantityRight = 0,
                        itemNameRight = "",
                        itemPriceRight = "$ " + "",
                        isItemRightVisiable = false,
                        isItemRightEnable = false,
                        quantityR = 0
                    });
                }
                if (isAmountItemsEven(n))
                {
                    for (int i = 0; i < n / 2; i++)
                    {
                        this.datagrid.Add(new ItemsModel()
                        {
                            height = this.Width / 2 + 25,
                            width = this.Width / 2 - 25,
                            imageSourceLeft = data.result.result[j].item_photo,
                            quantityLeft = 0,
                            itemNameLeft = data.result.result[j].item_name,
                            itemPriceLeft = "$ " + data.result.result[j].item_price.ToString(),
                            isItemLeftVisiable = true,
                            isItemLeftEnable = true,
                            quantityL = 0,

                            imageSourceRight = data.result.result[j + 1].item_photo,
                            quantityRight = 0,
                            itemNameRight = data.result.result[j + 1].item_name,
                            itemPriceRight = "$ " + data.result.result[j + 1].item_price.ToString(),
                            isItemRightVisiable = true,
                            isItemRightEnable = true,
                            quantityR = 0
                        });
                        j = j + 2;
                    }
                }
                else
                {
                    for (int i = 0; i < n / 2; i++)
                    {
                        this.datagrid.Add(new ItemsModel()
                        {
                            height = this.Width / 2 + 25,
                            width = this.Width / 2 - 25,
                            imageSourceLeft = data.result.result[j].item_photo,
                            quantityLeft = 0,
                            itemNameLeft = data.result.result[j].item_name,
                            itemPriceLeft = "$ " + data.result.result[j].item_price.ToString(),
                            isItemLeftVisiable = true,
                            isItemLeftEnable = true,
                            quantityL = 0,

                            imageSourceRight = data.result.result[j + 1].item_photo,
                            quantityRight = 0,
                            itemNameRight = data.result.result[j + 1].item_name,
                            itemPriceRight = "$ " + data.result.result[j + 1].item_price.ToString(),
                            isItemRightVisiable = true,
                            isItemRightEnable = true,
                            quantityR = 0
                        });
                        j = j + 2;
                    }
                    this.datagrid.Add(new ItemsModel()
                    {
                        height = this.Width / 2 + 25,
                        width = this.Width / 2 - 25,
                        imageSourceLeft = data.result.result[j].item_photo,
                        quantityLeft = 0,
                        itemNameLeft = data.result.result[j].item_name,
                        itemPriceLeft = "$ " + data.result.result[j].item_price.ToString(),
                        isItemLeftVisiable = true,
                        isItemLeftEnable = true,
                        quantityL = 0,

                        imageSourceRight = "",
                        quantityRight = 0,
                        itemNameRight = "",
                        itemPriceRight = "$ " + "",
                        isItemRightVisiable = false,
                        isItemRightEnable = false,
                        quantityR = 0
                    });
                }
            }
        }

        public bool isAmountItemsEven(int num)
        {
            bool result = false;
            if (num % 2 == 0) { result = true; }
            return result;
        }

        string GetWeekDay(Object sender)
        {
            var myStack = (StackLayout)sender;
            var myFrame = (Frame)myStack.Children[0];
            var myLabel = (Label)myFrame.Content;
            string day = myLabel.Text;
            return day;
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

        void CheckOutClickDeliveryDaysPage(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.CheckoutPage();
        }

        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            // SHOULDN'T MOVE SINCE YOU ARE IN THIS PAGE
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new CheckoutPage();
        }

        void InfoClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new InfoPage();
        }

        void ProfileClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new profileUser();
        }
    }
}
