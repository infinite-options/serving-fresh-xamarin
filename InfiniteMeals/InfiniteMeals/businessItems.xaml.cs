using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace InfiniteMeals
{
    // THE FOLLOWING CLASSES WILL STORE THE DATA FROM THE JSON OBJECT
    // REQUESTED BY THE BUSINESSITEMS ENDPOINT

    // CLASS 1 STORE THE INFORMATION ABOUT EACH ITEM OFFERED BY THE
    // BUSINESS
    public class Items
    {
        public string item_uid { get; set; }
        public string created_at { get; set; }
        public string itm_business_uid { get; set; }
        public string item_name { get; set; }
        public string item_desc { get; set; }
        public object item_unit { get; set; }
        public double item_price { get; set; }
        public string item_sizes { get; set; }
        public string item_photo { get; set; }
        public string favorite { get; set; }
        public string item_type { get; set; }
    }

    // CLASS 2 STORE THE INFORMATION ABOUT LIST OF ITEM OFFERED BY THE
    // BUSINESS
    // I HAD TO CHANGE Result TO r SINCERE THERE IS ANOTHER TYPE NAME RESULT
    public class r
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Items> result { get; set; }
        public string sql { get; set; }
    }

    // CLASS 3 STORE THE INFORMATION ABOUT A JSON OBJECT FROM THE
    // BUSINESSITEMS ENDPOINT
    public class ServingFreshBusinessItems
    {
        public string message { get; set; }
        public r result { get; set; }
    }

    // CLASS 4 CONTAINTS THE BINDING DATA FOR THE LISTVIEW CLASS
    // (CHANGE 4)
    //public class ItemModel
    //{
    //    public string imageSourceLeft { get; set; }
    //    public int quantityLeft { get; set; }
    //    public string itemNameLeft { get; set; }
    //    public string itemPriceLeft { get; set; }
    //    public bool isItemLeftVisiable { get; set; }
    //    public bool isItemLeftEnable { get; set; }

    //    public string imageSourceRight { get; set; }
    //    public int quantityRight { get; set; }
    //    public string itemNameRight { get; set; }
    //    public string itemPriceRight { get; set; }
    //    public bool isItemRightVisiable { get; set; }
    //    public bool isItemRightEnable { get; set; }
    //}

    public partial class businessItems : ContentPage
    {
        // WENT FROM ILIST<> TO OBSERVABLE COLLECTION (CHANGE 1)
        public ObservableCollection<ItemsModel> datagrid = new ObservableCollection<ItemsModel>();
        ServingFreshBusinessItems data = new ServingFreshBusinessItems();

        public businessItems(string day)
        {
            InitializeComponent();
            string businessID = "200-000004";
            GetBusinessItems(businessID);
            Title = day;
            itemList.RefreshCommand = new Command(() =>
            {
                GetBusinessItems(businessID);
                itemList.IsRefreshing = false;
            });
        }

        public async void GetBusinessItems(string bussiness_id)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + bussiness_id);
            var d = await client.GetStringAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + bussiness_id);
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
                // COMMENT THE FOLLOWING LINE OF CODE AS (CHANGE 3)
                // WENT FROM ITEMSOURCE = "{Binding datagrid}"
                // BindingContext = this;
                itemList.ItemsSource = datagrid;
            }
            else
            {
                await DisplayAlert("Alert!", "We apologize for the inconvenience. We encounter problems getting the data about this business. Our engineering team is working diligently to solve this issue. Thank you for your patience. ", "OK");
            }
        }

        public bool isAmountItemsEven(int num)
        {
            bool result = false;
            if (num % 2 == 0) { result = true; }
            return result;
        }

        public ItemsModel itemModelEmpty()
        {
            ItemsModel e = new ItemsModel();

            e.imageSourceLeft = "";
            e.quantityLeft = 0;
            e.itemNameLeft = "";
            e.itemPriceLeft = "";
            e.isItemLeftVisiable = false;
            e.isItemLeftEnable = false;

            e.imageSourceRight = "";
            e.quantityRight = 0;
            e.itemNameRight = "";
            e.itemPriceRight = "";
            e.isItemRightVisiable = false;
            e.isItemRightEnable = false;

            return e;
        }

        public ItemsModel itemModelEven(string imgLeft, string imgRight, string nameLeft, string nameRight, string priceLeft, string priceRight)
        {
            ItemsModel e = new ItemsModel();

            e.imageSourceLeft = imgLeft;
            e.quantityLeft = 0;
            e.itemNameLeft = nameLeft;
            e.itemPriceLeft = "$ " + priceLeft;
            e.isItemLeftVisiable = true;
            e.isItemLeftEnable = true;

            e.imageSourceRight = imgRight;
            e.quantityRight = 0;
            e.itemNameRight = nameRight;
            e.itemPriceRight = "$ " + priceRight;
            e.isItemRightVisiable = true;
            e.isItemRightEnable = true;

            return e;
        }

        public ItemsModel itemModelOdd(string imgLeft, string imgRight, string nameLeft, string nameRight, string priceLeft, string priceRight)
        {
            ItemsModel e = new ItemsModel();

            e.imageSourceLeft = imgLeft;
            e.quantityLeft = 0;
            e.itemNameLeft = nameLeft;
            e.itemPriceLeft = "$ " + priceLeft;
            e.isItemLeftVisiable = true;
            e.isItemLeftEnable = true;

            e.imageSourceRight = imgRight;
            e.quantityRight = 0;
            e.itemNameRight = nameRight;
            e.itemPriceRight = "$ " + priceRight;
            e.isItemRightVisiable = false;
            e.isItemRightEnable = false;

            return e;
        }

        void SubtractItemLeft(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;

            if (itemModelObject != null)
            {
                if (itemModelObject.quantityL != 0)
                {
                    itemModelObject.quantityL -= 1;
                }
            }
        }

        void AddItemLeft(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;

            if (itemModelObject != null)
            {
                itemModelObject.quantityL += 1;
            }
        }

        void SubtractItemRight(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;

            if (itemModelObject != null)
            {
                if (itemModelObject.quantityR != 0)
                {
                    itemModelObject.quantityR -= 1;
                }
            }
        }

        void AddItemRight(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;

            if (itemModelObject != null)
            {
                itemModelObject.quantityR += 1;
            }
        }
    }
}
