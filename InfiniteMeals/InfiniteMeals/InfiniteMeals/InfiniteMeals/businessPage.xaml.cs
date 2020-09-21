using System;
using System.Collections.Generic;

using Xamarin.Forms;

using InfiniteMeals.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;
using InfiniteMeals.Kitchens.Model;

namespace InfiniteMeals
{
    public partial class businessPage : ContentPage
    {
        public ObservableCollection<ItemsModel> datagrid = new ObservableCollection<ItemsModel>();
        //ServingFreshBusinessItems data = new ServingFreshBusinessItems();

        public businessPage()
        {
            InitializeComponent();
            // GetData();
        }

        // THIS FUNCTION WILL CALL THE ENDPOINT BASED ON THE DAY SELECTED

        //async void GetData()
        //{
        //    var client = new HttpClient();
        //    var response = await client.GetAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + "200-000004");
        //    var d = await client.GetStringAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/itemsByBusiness/" + "200-000004");
        //    if (response.IsSuccessStatusCode)
        //    {
        //       // data = JsonConvert.DeserializeObject<ServingFreshBusinessItems>(d);

        //        // COMMENT THE FOLLOWING LINE OF CODE AS (CHANGE 2)
        //        // datagrid = new List<ItemModel>();
        //        this.datagrid.Clear();
        //        int n = data.result.result.Count;
        //        int j = 0;
        //        if (n == 0)
        //        {
        //            this.datagrid.Add(new ItemsModel()
        //            {
        //                height = this.Width / 2 + 25,
        //                width = this.Width / 2 - 25,
        //                imageSourceLeft = "",
        //                quantityLeft = 0,
        //                itemNameLeft = "",
        //                itemPriceLeft = "$ " + "",
        //                isItemLeftVisiable = false,
        //                isItemLeftEnable = false,
        //                quantityL = 0,

        //                imageSourceRight = "",
        //                quantityRight = 0,
        //                itemNameRight = "",
        //                itemPriceRight = "$ " + "",
        //                isItemRightVisiable = false,
        //                isItemRightEnable = false,
        //                quantityR = 0
        //            });
        //        }
        //        if (isAmountItemsEven(n))
        //        {
        //            for (int i = 0; i < n / 2; i++)
        //            {
        //                this.datagrid.Add(new ItemsModel()
        //                {
        //                    height = this.Width / 2 + 25,
        //                    width = this.Width / 2 - 25,
        //                    imageSourceLeft = data.result.result[j].item_photo,
        //                    quantityLeft = 0,
        //                    itemNameLeft = data.result.result[j].item_name,
        //                    itemPriceLeft = "$ " + data.result.result[j].item_price.ToString(),
        //                    isItemLeftVisiable = true,
        //                    isItemLeftEnable = true,
        //                    quantityL = 0,

        //                    imageSourceRight = data.result.result[j + 1].item_photo,
        //                    quantityRight = 0,
        //                    itemNameRight = data.result.result[j + 1].item_name,
        //                    itemPriceRight = "$ " + data.result.result[j + 1].item_price.ToString(),
        //                    isItemRightVisiable = true,
        //                    isItemRightEnable = true,
        //                    quantityR = 0
        //                });
        //                j = j + 2;
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < n / 2; i++)
        //            {
        //                this.datagrid.Add(new ItemsModel()
        //                {
        //                    height = this.Width / 2 + 25,
        //                    width = this.Width / 2 - 25,
        //                    imageSourceLeft = data.result.result[j].item_photo,
        //                    quantityLeft = 0,
        //                    itemNameLeft = data.result.result[j].item_name,
        //                    itemPriceLeft = "$ " + data.result.result[j].item_price.ToString(),
        //                    isItemLeftVisiable = true,
        //                    isItemLeftEnable = true,
        //                    quantityL = 0,

        //                    imageSourceRight = data.result.result[j + 1].item_photo,
        //                    quantityRight = 0,
        //                    itemNameRight = data.result.result[j + 1].item_name,
        //                    itemPriceRight = "$ " + data.result.result[j + 1].item_price.ToString(),
        //                    isItemRightVisiable = true,
        //                    isItemRightEnable = true,
        //                    quantityR = 0
        //                });
        //                j = j + 2;
        //            }
        //            this.datagrid.Add(new ItemsModel()
        //            {
        //                height = this.Width / 2 + 25,
        //                width = this.Width / 2 - 25,
        //                imageSourceLeft = data.result.result[j].item_photo,
        //                quantityLeft = 0,
        //                itemNameLeft = data.result.result[j].item_name,
        //                itemPriceLeft = "$ " + data.result.result[j].item_price.ToString(),
        //                isItemLeftVisiable = true,
        //                isItemLeftEnable = true,
        //                quantityL = 0,

        //                imageSourceRight = "",
        //                quantityRight = 0,
        //                itemNameRight = "",
        //                itemPriceRight = "$ " + "",
        //                isItemRightVisiable = false,
        //                isItemRightEnable = false,
        //                quantityR = 0
        //            });
        //        }
        //        // COMMENT THE FOLLOWING LINE OF CODE AS (CHANGE 3)
        //        // WENT FROM ITEMSOURCE = "{Binding datagrid}"
        //        // BindingContext = this;
        //        //itemList.ItemsSource = data;
        //    }
        //}

        public bool isAmountItemsEven(int num)
        {
            bool result = false;
            if (num % 2 == 0) { result = true; }
            return result;
        }
    }
}
