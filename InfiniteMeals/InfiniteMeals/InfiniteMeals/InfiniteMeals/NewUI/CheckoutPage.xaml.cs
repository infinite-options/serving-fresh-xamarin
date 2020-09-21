using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace InfiniteMeals.NewUI
{
    public class ItemObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string item_uid { get; set; }
        public string name { get; set; }
        public int qty { get; set; }
        public double price { get; set; }
        public string total_price { get { return "$" + (qty * price).ToString("N2"); } }
        public void increase_qty()
        {
            qty++;
            PropertyChanged(this, new PropertyChangedEventArgs("qty"));
            PropertyChanged(this, new PropertyChangedEventArgs("total_price"));
        }
        public void decrease_qty()
        {
            if (qty > 0) qty--;
            PropertyChanged(this, new PropertyChangedEventArgs("qty"));
            PropertyChanged(this, new PropertyChangedEventArgs("total_price"));
        }

    }
    public class PurchaseDataObject
    {
        public string pur_customer_uid { get; set; }
        public string pur_business_uid { get; set; }
        public string items { get; set; }
        public string order_instructions { get; set; }
        public string delivery_instructions { get; set; }
        public string order_type { get; set; }
        public string delivery_first_name { get; set; }
        public string delivery_last_name { get; set; }
        public string delivery_phone_num { get; set; }
        public string delivery_email { get; set; }
        public string delivery_address { get; set; }
        public string delivery_unit { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_zip { get; set; }
        public string delivery_latitude { get; set; }
        public string delivery_longitude { get; set; }
        public string purchase_notes { get; set; }
        public string start_delivery_date { get; set; }
        public string pay_coupon_id { get; set; }
        public string amount_due { get; set; }
        public string amount_discount { get; set; }
        public string amount_paid { get; set; }
        public string info_is_Addon { get; set; }
        public string cc_num { get; set; }
        public string cc_exp_date { get; set; }
        public string cc_cvv { get; set; }
        public string cc_zip { get; set; }
        public string charge_id { get; set; }
        public string payment_type { get; set; }
    }
    public class PurchaseResponse
    {
        public int code { get; set; }
        public string message { get; set; }
        public string sql { get; set; }
    }
    public partial class CheckoutPage : ContentPage
    {
        public PurchaseDataObject purchaseObject;
        public ObservableCollection<ItemObject> cartItems = new ObservableCollection<ItemObject>();
        public double subtotal;
        public double discount;
        public double delivery_fee;
        public double taxes;
        public double total;
        public CheckoutPage()
        {
            InitializeComponent();
            cartItems.Add(new ItemObject()
            {
                qty = 1,
                name = "Boysenberry Pie (each)",
                price = 13.99,
                item_uid = "320-000001"
            });
            cartItems.Add(new ItemObject()
            {
                qty = 1,
                name = "Early Girl Tomatoes (per lb)",
                price = 3.99,
                item_uid = "320-000002"
            });
            cartItems.Add(new ItemObject()
            {
                qty = 2,
                name = "Almonds (per lb)",
                price = 4.00,
                item_uid = "320-000003"
            });
            purchaseObject = new PurchaseDataObject()
            {
                pur_customer_uid = "100-000009",
                pur_business_uid = "200-000001",
                items = "",
                order_instructions = "fast",
                delivery_instructions = "Keep Fresh",
                order_type = "meal",
                delivery_first_name = "xyz",
                delivery_last_name = "abc",
                delivery_phone_num = "6197872089",
                delivery_email = "xyz@gmail.com",
                delivery_address = "790 Carrywood Way",
                delivery_unit = "9",
                delivery_city = "San Jose",
                delivery_state = "CA",
                delivery_zip = "95120",
                delivery_latitude = "37.2271302",
                delivery_longitude = "-121.8891617",
                purchase_notes = "purchase_notes"
            };
            DeliveryAddress1.Text = purchaseObject.delivery_address;
            DeliveryAddress2.Text = purchaseObject.delivery_city+", "
                +purchaseObject.delivery_state+", "+purchaseObject.delivery_zip;
            FullName.Text = purchaseObject.delivery_first_name + " " + purchaseObject.delivery_last_name;
            PhoneNumber.Text = purchaseObject.delivery_phone_num;
            EmailAddress.Text = purchaseObject.delivery_email;
            CartItems.ItemsSource = cartItems;
            CartItems.HeightRequest = 56 * cartItems.Count;
            updateTotals();
        }
        public void updateTotals()
        {
            subtotal = 0.0;
            foreach (ItemObject item in cartItems)
            {
                subtotal += (item.qty * item.price);
            }
            SubTotal.Text = "$" + subtotal.ToString("N2");
            discount = subtotal * .1;
            Discount.Text = "-$" + discount.ToString("N2");
            delivery_fee = 1.50;
            DeliveryFee.Text = "$" + delivery_fee.ToString("N2");
            taxes = subtotal * 0.085;
            Taxes.Text = "$" + taxes.ToString("N2");
            total = subtotal - discount + delivery_fee + taxes;
            GrandTotal.Text = "$" + total.ToString("N2");
        }
        public string createItemJSON(ItemObject item)
        {
            return "{\"item_uid\": " + "\"" + item.item_uid + "\"," + "\"qty\": " + "\"" + item.qty +
             "\"," + "\"name\": " + "\"" + item.name + "\"," + "\"price\": " + "\"" + item.price + "\"}";
        }
        public string createItemsJSON(IList<ItemObject> items)
        {
            string itemsJSON = "[";
            foreach (ItemObject item in items)
            {
                if (item.qty == 0) continue;
                itemsJSON += createItemJSON(item);
                if (items.IndexOf(item) != items.Count-1) itemsJSON += ",";
            }
            itemsJSON += "]";
            return itemsJSON;
        }
        public async void checkoutAsync(object sender, EventArgs e)
        {
            purchaseObject.items = createItemsJSON(cartItems);
            purchaseObject.start_delivery_date = DateTime.Today.ToString();
            purchaseObject.pay_coupon_id = "";
            purchaseObject.amount_due = total.ToString("N2");
            purchaseObject.amount_discount = discount.ToString("N2");
            purchaseObject.amount_paid = total.ToString("N2");
            purchaseObject.info_is_Addon = "FALSE";
            purchaseObject.cc_num = "4545";
            purchaseObject.cc_exp_date = DateTime.Today.AddYears(1).ToString();
            purchaseObject.cc_cvv = "666";
            purchaseObject.cc_zip = purchaseObject.delivery_zip;
            purchaseObject.charge_id = "";
            purchaseObject.payment_type = ((Button)sender).Text == "Checkout with Paypal" ? "PAYPAL" : "STRIPE";
            var purchaseString = JsonConvert.SerializeObject(purchaseObject);
            Console.WriteLine(purchaseString);
            var purchaseMessage = new StringContent(purchaseString, Encoding.UTF8, "application/json");
            var httpClient = new HttpClient();
            var request = new HttpRequestMessage();
            request.Method = HttpMethod.Post;
            request.Content = purchaseMessage;
            var httpResponse = await httpClient.PostAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/purchase_Data_SF", purchaseMessage);
            string responseStr = await httpResponse.Content.ReadAsStringAsync();
            PurchaseResponse data = JsonConvert.DeserializeObject<PurchaseResponse>(responseStr);
            Console.WriteLine(data.message);
        }
        public void increase_qty(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            TapGestureRecognizer tgr = (TapGestureRecognizer)l.GestureRecognizers[0];
            ItemObject item = (ItemObject)tgr.CommandParameter;
            if (item != null) item.increase_qty();
            Console.WriteLine(createItemsJSON((ObservableCollection<ItemObject>)CartItems.ItemsSource));
            updateTotals();
        }
        public void decrease_qty(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            TapGestureRecognizer tgr = (TapGestureRecognizer)l.GestureRecognizers[0];
            ItemObject item = (ItemObject)tgr.CommandParameter;
            if (item != null) item.decrease_qty();
            Console.WriteLine(createItemsJSON((ObservableCollection<ItemObject>)CartItems.ItemsSource));
            updateTotals();
        }
        public void openHistory(object sender, EventArgs e)
        {
            Navigation.PushAsync(new HistoryPage());
        }
        public void openRefund(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RefundPage());
        }
        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new StartPage();
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            // Already on orders page
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
