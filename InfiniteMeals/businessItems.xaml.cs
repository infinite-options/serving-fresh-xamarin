using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using InfiniteMeals.Models;
using InfiniteMeals.NewUI;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class businessItems : ContentPage
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

        public class GetItemPost
        {
            public IList<string> type { get; set; }
            public IList<string> ids { get; set; }
        }

        // THIS VARIABLE SHOULD BE THE COMPLETE SOLUTION THAT ZACK MAY USE
        public ObservableCollection<ItemsModel> datagrid = new ObservableCollection<ItemsModel>();
        ServingFreshBusinessItems data = new ServingFreshBusinessItems();

        public class ItemPurchased
        {
            public string pur_business_uid { get; set; }
            public string item_uid { get; set; }
            public string item_name { get; set; }
            public int item_quantity { get; set; }
            public double item_price { get; set; }
        }

        public IDictionary<string, ItemPurchased> order = new Dictionary<string,ItemPurchased>();
        public int totalCount = 0;
        //ServingFreshBusinessItems data = new ServingFreshBusinessItems();

        public businessItems(List<string> types, List<string> b_uids, string day)
        {
            InitializeComponent();
            GetData(types, b_uids);
            titlePage.Text = day;
            itemList.ItemsSource = datagrid;
            CartTotal.Text = totalCount.ToString();
        }

        private async void GetData(List<string> types, List<string> b_uids)
        {
            GetItemPost post = new GetItemPost();
            post.type = types;
            post.ids = b_uids;

            var client = new HttpClient();
            var getItemsString = JsonConvert.SerializeObject(post);
            var getItemsStringMessage = new StringContent(getItemsString, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.Content = getItemsStringMessage;

            var httpResponse = await client.PostAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/getItems", getItemsStringMessage);
            string responseStr = await httpResponse.Content.ReadAsStringAsync();
            //ServingFreshBusinessItems data = JsonConvert.DeserializeObject<ServingFreshBusinessItems>(responseStr);
            //var response = await client.GetAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/getItems/" + weekDay);
            //var d = await client.GetStringAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/getItems/" + weekDay);
            //Console.WriteLine("This is the data received for items = " + d);
            //Console.WriteLine(response.IsSuccessStatusCode);
            Console.WriteLine(responseStr);
            if (responseStr != null)
            {
                //string result = response.Content.ReadAsStringAsync().Result;
                data = JsonConvert.DeserializeObject<ServingFreshBusinessItems>(responseStr);

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
            ItemPurchased itemSelected = new ItemPurchased();
            if (itemModelObject != null)
            {
                if (itemModelObject.quantityL != 0)
                {
                    itemModelObject.quantityL -= 1;
                    totalCount -= 1;
                    CartTotal.Text = totalCount.ToString();
                    if (order != null)
                    {
                        if (order.ContainsKey(itemModelObject.itemNameLeft))
                        {
                            var itemToUpdate = order[itemModelObject.itemNameLeft];
                            itemToUpdate.item_quantity = itemModelObject.quantityL;
                            order[itemModelObject.itemNameLeft] = itemToUpdate;
                        }
                        else
                        {
                            itemSelected.pur_business_uid = itemModelObject.itm_business_uidLeft;
                            itemSelected.item_uid = itemModelObject.item_uidLeft;
                            itemSelected.item_name = itemModelObject.itemNameLeft;
                            itemSelected.item_quantity = itemModelObject.quantityL;
                            itemSelected.item_price = Convert.ToDouble(itemModelObject.itemPriceLeft.Substring(1).Trim());
                            order.Add(itemModelObject.itemNameLeft, itemSelected);
                        }
                    }
                }
            }
        }

        void AddItemLeft(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;
            ItemPurchased itemSelected = new ItemPurchased();
            if (itemModelObject != null)
            {
                itemModelObject.quantityL += 1;
                totalCount += 1;
                CartTotal.Text = totalCount.ToString();
                if (order != null)
                {
                    if (order.ContainsKey(itemModelObject.itemNameLeft))
                    {
                        var itemToUpdate = order[itemModelObject.itemNameLeft];
                        itemToUpdate.item_quantity = itemModelObject.quantityL;
                        order[itemModelObject.itemNameLeft] = itemToUpdate;
                    }
                    else
                    {
                        itemSelected.pur_business_uid = itemModelObject.itm_business_uidLeft;
                        itemSelected.item_uid = itemModelObject.item_uidLeft;
                        itemSelected.item_name = itemModelObject.itemNameLeft;
                        itemSelected.item_quantity = itemModelObject.quantityL;
                        itemSelected.item_price = Convert.ToDouble(itemModelObject.itemPriceLeft.Substring(1).Trim());
                        order.Add(itemModelObject.itemNameLeft, itemSelected);
                    }
                }
            }
        }

        void SubtractItemRight(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;
            ItemPurchased itemSelected = new ItemPurchased();
            if (itemModelObject != null)
            {
                if (itemModelObject.quantityR != 0)
                {
                    itemModelObject.quantityR -= 1;
                    totalCount -= 1;
                    CartTotal.Text = totalCount.ToString();
                    if (order.ContainsKey(itemModelObject.itemNameRight))
                    {
                        var itemToUpdate = order[itemModelObject.itemNameRight];
                        itemToUpdate.item_quantity = itemModelObject.quantityR;
                        order[itemModelObject.itemNameRight] = itemToUpdate;
                    }
                    else
                    {
                        itemSelected.pur_business_uid = itemModelObject.itm_business_uidRight;
                        itemSelected.item_uid = itemModelObject.item_uidRight;
                        itemSelected.item_name = itemModelObject.itemNameRight;
                        itemSelected.item_quantity = itemModelObject.quantityR;
                        itemSelected.item_price = Convert.ToDouble(itemModelObject.itemPriceRight.Substring(1).Trim());
                        order.Add(itemModelObject.itemNameRight, itemSelected);
                    }
                }
            }
        }

        void AddItemRight(System.Object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var itemModelObject = (ItemsModel)button.CommandParameter;
            ItemPurchased itemSelected = new ItemPurchased();
            if (itemModelObject != null)
            {
                itemModelObject.quantityR += 1;
                totalCount += 1;
                CartTotal.Text = totalCount.ToString();
                if (order.ContainsKey(itemModelObject.itemNameRight))
                {
                    var itemToUpdate = order[itemModelObject.itemNameRight];
                    itemToUpdate.item_quantity = itemModelObject.quantityR;
                    order[itemModelObject.itemNameRight] = itemToUpdate;
                }
                else
                {
                    itemSelected.pur_business_uid = itemModelObject.itm_business_uidRight;
                    itemSelected.item_uid = itemModelObject.item_uidRight;
                    itemSelected.item_name = itemModelObject.itemNameRight;
                    itemSelected.item_quantity = itemModelObject.quantityR;
                    itemSelected.item_price = Convert.ToDouble(itemModelObject.itemPriceRight.Substring(1).Trim());
                    order.Add(itemModelObject.itemNameRight, itemSelected);
                }
            }
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

        void CheckOutClickBusinessPage(System.Object sender, System.EventArgs e)
        {
            Console.WriteLine("THIS IS THE LIST OF ITEMS I ORDERED");
            int itemNumber = 1;
            foreach (string key in order.Keys)
            {
                Console.WriteLine("ITEM NUMBER IN THE LIST: " + itemNumber);
                Console.WriteLine("ITEM NUMBER IN THE BUSINESS_ID: " + order[key].pur_business_uid);
                Console.WriteLine("ITEM NUMBER IN THE ITEM_ID: " + order[key].item_uid);
                Console.WriteLine("ITEM NAME = " + order[key].item_name);
                Console.WriteLine("ITEMS QUANTITY = " + order[key].item_quantity);
                Console.WriteLine("ITEMS PRICE = " + order[key].item_price);
                itemNumber++;
            }

            Application.Current.MainPage = new NewUI.CheckoutPage(order);
        }

        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.CheckoutPage(order);
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
