using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using InfiniteMeals.Models;
using InfiniteMeals.NewUI;
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

    public partial class businessItems : ContentPage
    {
        // WENT FROM ILIST<> TO OBSERVABLE COLLECTION (CHANGE 1)
        public ObservableCollection<ItemsModel> datagrid = new ObservableCollection<ItemsModel>();
        //ServingFreshBusinessItems data = new ServingFreshBusinessItems();

        public businessItems(ObservableCollection<ItemsModel> data, string day)
        {
            InitializeComponent();
            titlePage.Text = day;
            itemList.ItemsSource = data;
        }

        async void Open_Checkout(System.Object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new CheckoutPage());
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
            Application.Current.MainPage = new NewUI.CheckoutPage();
        }

        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new OrdersPage();
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
