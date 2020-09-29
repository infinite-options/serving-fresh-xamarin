using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using InfiniteMeals.Meals.Model;
using InfiniteMeals.NewUI;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class CarlosCategoriesPage : ContentPage
    {
        public class Items
        {
            public string item_uid { get; set; }
            public string created_at { get; set; }
            public string itm_business_uid { get; set; }
            public string item_name { get; set; }
            public object item_status { get; set; }
            public string item_type { get; set; }
            public string item_desc { get; set; }
            public object item_unit { get; set; }
            public double item_price { get; set; }
            public string item_sizes { get; set; }
            public string favorite { get; set; }
            public string item_photo { get; set; }
            public object exp_date { get; set; }
            public string business_delivery_hours { get; set; }
        }

        public class ServingFreshBusinessItems
        {
            public string message { get; set; }
            public int code { get; set; }
            public IList<Items> result { get; set; }
            public string sql { get; set; }
        }

        public ObservableCollection<MyLabel> datagrid1 = new ObservableCollection<MyLabel>();
        public ObservableCollection<MyFarmersMarket> datagrid2 = new ObservableCollection<MyFarmersMarket>();
        public ObservableCollection<MyFarm> datagrid3 = new ObservableCollection<MyFarm>();
        public ObservableCollection<MyProduce> datagrid4 = new ObservableCollection<MyProduce>();
        ObservableCollection<ItemsModel> datagrid = new ObservableCollection<ItemsModel>();
        ServingFreshBusinessItems data = new ServingFreshBusinessItems();

        public class MyLabel
        {
            public string day1 { get; set; }
            public string month1 { get; set; }
            public string date1 { get; set; }

            public string day2 { get; set; }
            public string month2 { get; set; }
            public string date2 { get; set; }

            public string day3 { get; set; }
            public string month3 { get; set; }
            public string date3 { get; set; }

        }

        public class MyFarmersMarket
        {
            public string source1 { get; set; }
            public string source2 { get; set; }
            public string source3 { get; set; }
        }

        public class MyProduce
        {
            public string source1 { get; set; }
            public string source2 { get; set; }
            public string source3 { get; set; }
        }

        public class MyFarm
        {
            public string farm1 { get; set; }
            public string farm2 { get; set; }
            public string farm3 { get; set; }
        }

        public CarlosCategoriesPage()
        {
            InitializeComponent();
            MyLabel myFirstLabels = new MyLabel
            {
                day1 = "Sunday",
                month1 = "Sept",
                date1 = "27",

                day2 = "Monday",
                month2 = "Sept",
                date2 = "28",

                day3 = "Tuesday",
                month3 = "Sept",
                date3 = "29",
            };
            MyLabel mySecondLabels = new MyLabel
            {
                day1 = "Wednesday",
                month1 = "Sept",
                date1 = "30",

                day2 = "Thursday",
                month2 = "Oct",
                date2 = "1",

                day3 = "Friday",
                month3 = "Oct",
                date3 = "2",
            };
            MyLabel myThirdLabels = new MyLabel
            {
                day1 = "Saturday",
                month1 = "Oct",
                date1 = "3"
            };
            datagrid1.Add(myFirstLabels);
            datagrid1.Add(mySecondLabels);
            datagrid1.Add(myThirdLabels);

            myCarouseView0.ItemsSource = datagrid1;

            MyFarmersMarket myFarmersMarket1 = new MyFarmersMarket
            {
                source1 = "californiaMarkets.png",
                source2 = "champsMarket.png",
                source3 = "SantaRosaMarkets.jpg",
            };

            MyFarmersMarket myFarmersMarket2 = new MyFarmersMarket
            {
                source1 = "PCFMA.png",
                source2 = "urbanMarkets.jpeg",
            };

            MyFarm myFarm1 = new MyFarm
            {
                farm1 = "Esquivel Farm",
                farm2 = "Wildstone Bakery",
                farm3 = "Avila Farms",
            };

            MyFarm myFarm2 = new MyFarm
            {
                farm1 = "Garcia Farms",
                farm2 = "Resendiz Farms",
                farm3 = "Almaden Farms",
            };

            MyProduce produce1 = new MyProduce
            {
                source1 = "OrangeIcon",
                source2 = "VegIcon",
                source3 = "DonutIcon",
            };

            MyProduce produce2 = new MyProduce
            {
                source1 = "BreadIcon",
            };

            datagrid2.Add(myFarmersMarket1);
            datagrid2.Add(myFarmersMarket2);

            datagrid3.Add(myFarm1);
            datagrid3.Add(myFarm2);

            datagrid4.Add(produce1);
            datagrid4.Add(produce2);

            myCarouseView0.ItemsSource = datagrid1;
            myCarouseView1.ItemsSource = datagrid2;
            myCarouseView2.ItemsSource = datagrid3;
            myCarouseView3.ItemsSource = datagrid4;
            //MessagesListView.ItemsSource = datagrid;
        }

        async void Open_Checkout(Object sender, EventArgs e)
        {

            await Navigation.PushAsync(new CheckoutPage(null));
        }

        //void Open_Farm(Object sender, EventArgs e)
        //{
            
        //    Application.Current.MainPage = new businessItems(datagrid, "Monday");
        //}

        private async Task GetData(string weekDay)
        {
            // THE "200-000004" WOULD GET REPLACE BY THE "weekDay" STRING
            var client = new HttpClient();
            var response = await client.GetAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/getItems/" + weekDay);
            var d = await client.GetStringAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/getItems/" + weekDay);
            Console.WriteLine("This is the data received for items = " + d);
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<ServingFreshBusinessItems>(result);

                // COMMENT THE FOLLOWING LINE OF CODE AS (CHANGE 2)
                // datagrid = new List<ItemModel>();
                this.datagrid.Clear();
                int n = data.result.Count;
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
                            imageSourceLeft = data.result[j].item_photo,
                            item_uidLeft = data.result[j].item_uid,
                            itm_business_uidLeft = data.result[j].itm_business_uid,
                            quantityLeft = 0,
                            itemNameLeft = data.result[j].item_name,
                            itemPriceLeft = "$ " + data.result[j].item_price.ToString(),
                            isItemLeftVisiable = true,
                            isItemLeftEnable = true,
                            quantityL = 0,

                            imageSourceRight = data.result[j + 1].item_photo,
                            item_uidRight = data.result[j + 1].item_uid,
                            itm_business_uidRight = data.result[j + 1].itm_business_uid,
                            quantityRight = 0,
                            itemNameRight = data.result[j + 1].item_name,
                            itemPriceRight = "$ " + data.result[j + 1].item_price.ToString(),
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
                            imageSourceLeft = data.result[j].item_photo,
                            item_uidLeft = data.result[j].item_uid,
                            itm_business_uidLeft = data.result[j].itm_business_uid,
                            quantityLeft = 0,
                            itemNameLeft = data.result[j].item_name,
                            itemPriceLeft = "$ " + data.result[j].item_price.ToString(),
                            isItemLeftVisiable = true,
                            isItemLeftEnable = true,
                            quantityL = 0,

                            imageSourceRight = data.result[j + 1].item_photo,
                            item_uidRight = data.result[j + 1].item_uid,
                            itm_business_uidRight = data.result[j + 1].itm_business_uid,
                            quantityRight = 0,
                            itemNameRight = data.result[j + 1].item_name,
                            itemPriceRight = "$ " + data.result[j + 1].item_price.ToString(),
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
                        imageSourceLeft = data.result[j].item_photo,
                        item_uidLeft = data.result[j].item_uid,
                        itm_business_uidLeft = data.result[j].itm_business_uid,
                        quantityLeft = 0,
                        itemNameLeft = data.result[j].item_name,
                        itemPriceLeft = "$ " + data.result[j].item_price.ToString(),
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

        void MondayClick(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine("You clicked on Monday");
            GetData("Monday");
            Application.Current.MainPage = new businessItems(datagrid, "Monday");
        }

        void TuesdayClick(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine("You clicked on Tuesday");
            GetData("Monday");
            Application.Current.MainPage = new businessItems(datagrid, "Monday");
        }

        void WednesdayClick(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine("You clicked on Wednesday");
            GetData("Monday");
            Application.Current.MainPage = new businessItems(datagrid, "Monday");
        }

        void CheckOutClickDeliveryDaysPage(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.CheckoutPage(null);
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
