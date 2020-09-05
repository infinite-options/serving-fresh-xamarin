using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Xamarin.Forms;
using Newtonsoft.Json;
using InfiniteMeals.Meals.Model;
using System.Collections.ObjectModel;
using Newtonsoft.Json.Linq;
using Stripe;
using Application = Xamarin.Forms.Application;
using Acr.UserDialogs;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using System.Xml.Linq;
using System.Net;
using System.Linq;
using Plugin.LatestVersion;
using InfiniteMeals.Checkout;

namespace InfiniteMeals
{
    public class OrderInfo
    {
        public string order_id = "";
        public string email = "";
        public string name = "";
        public string phone = "";
        public string street = "";
        public string city = "";
        public string state = "";
        public string zipCode = "";
        public string totalAmount = "";
        public bool paid = false;
        public bool notification_enabled = false;
        public string paymentType = "cash";
        public string address_unit = "";
        public string delivery_instructions = "";
        //public string deliveryTime = "5:00 PM";
        //public string mealOption1 = "1";
        //public string mealOption2 = "1";
        public string kitchen_id = "";
        public string addressLongitude = "0";
        public string addressLatitude = "0";
        public string appVersion = "";
        public List<Dictionary<string, string>> ordered_items = new List<Dictionary<string, string>>();
    }

    // CLASS THAT STORES THE DATA FOR THE ORDER MADE
    // WRITE DATA TO PURCHASE TABLE AND PAYMENT TABLE
    public class OrderObject
    {
        // Not so sure if I set the customer_id and or order_id
        public string customer_uid { get; set; }
        public string business_uid { get; set; }
        public string delivery_first_name { get; set; }
        public string delivery_last_name { get; set; }
        public string delivery_email { get; set; }
        public string delivery_phone { get; set; }
        public string delivery_address { get; set; }
        public string delivery_unit { get; set; }
        public string delivery_city { get; set; }
        public string delivery_state { get; set; }
        public string delivery_zip { get; set; }
        public string delivery_instructions { get; set; }
        public string delivery_longitude { get; set; }
        public string delivery_latitude { get; set; }
        public string items { get; set; }
        public string order_instructions { get; set; }
        public string purchase_notes { get; set; }
        public string amount_due { get; set; }
        public string amount_discount { get; set; }
        public string amount_paid { get; set; }

        // EVERYTHING BEFORE THIS POINT IS WORKING
        public bool paid = false;
        public bool notification_enabled = false;
        public string paymentType = "cash";
        public string appVersion = "";
    }

    public class CouponInfo
    {
        public double couponCredit = 0.0;
        public string couponID = "";
    }

    public class CouponObject
    {
        public string coupon_uid { get; set; }
        public int num_used { get; set; }
    }

    public class message
    {
        public string client_secret { get; set; }
        public string id { get; set; }
    }

    public class UpdateRegistrationInfo
    {
        public string guid { get; set; }
        public string tags { get; set; }
    }
    public partial class CheckOutPage : ContentPage
    {
        // https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/couponDetails/Jane6364
        OrderInfo currentOrder = new OrderInfo();
        CouponInfo couponInfo = new CouponInfo();
        CouponObject couponObject = new CouponObject();

        string result = null;
        ObservableCollection<MealsModel> mealsOrdered = new ObservableCollection<MealsModel>();

        double totalCostsForMeals = 0;

        double calculatedTaxAmount = 0;

        string kitchenZipcode;
        string availableZipcode;

        int calls = 0;
        /*public Label couponLabel = new Label() { Text = String.Concat("Coupon "), FontSize = 12};
        var couponAmount = new Label() { Text = "$ " + FormatCurrency(couponCredit.ToString()), FontSize = 12, HorizontalTextAlignment = TextAlignment.End}*/


        public Label couponLabel = new Label() { Text = "Coupon", FontSize = 12};

        public Label couponAmount = new Label() { Text = "$ 0.00" , FontSize = 12, HorizontalTextAlignment = TextAlignment.End};

        public Label deliveryLabel = new Label() { Text = "Delivery charges", FontSize = 12 };
        public Label deliveryAmount = new Label() { Text = "$ 5.00", FontSize = 12, HorizontalTextAlignment = TextAlignment.End };

        public Label totalAmountTextLabel = new Label() { Text = "", FontSize = 14, FontAttributes = FontAttributes.Bold };
        public Label totalAmountLabel = new Label() { Text = "", FontSize = 14, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.End };


        //var firstNameField = new Entry() { Placeholder="First name", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };
        //var lastNameField = new Entry() { Placeholder = "Last name", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };
        public Entry fullNameField = new Entry() { Placeholder = "Full name", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };
        public Entry emailField = new Entry() { Placeholder = "Email", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Email };
        public Entry phoneField = new Entry() { Placeholder = "Phone", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Numeric };


        public Entry streetAddressField = new Entry() { Placeholder = "Street Address", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };
        public Entry cityField = new Entry() { Placeholder = "City", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };

        public Entry stateField = new Entry() { Placeholder = "State", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 100 };
        public Entry zipCodeField = new Entry() { Placeholder = "Zip Code", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 170, Keyboard = Keyboard.Numeric };
        public Entry addressUnitField = new Entry() { Placeholder = "Address Unit", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 170};
        public Entry deliveryInstructionField = new Entry() { Placeholder = "Delivery Instruction (e.g. Gate Code, etc)", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 170 };

        public Entry couponCodeField = new Entry() { Placeholder = "Coupon Code", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Text };
        public Button applyCouponButton = new Button() { Text = "Apply", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HeightRequest = 45, BorderWidth = 0.5, BorderColor = Color.Gray, TextColor = Color.FromHex("#a0050f"), CornerRadius = 10,BackgroundColor = Color.Transparent};

        public Entry CreditCardNumberField = new Entry() { Placeholder = "Credit Card Number", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Numeric };
        public Entry ExpMonthField = new Entry() { Placeholder = "MM", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Numeric };
        public Entry ExpYearField = new Entry() { Placeholder = "YY", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Numeric };
        public Entry CVCField = new Entry() { Placeholder = "CVC", VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.FillAndExpand, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Numeric };

        public CheckOutPage(ObservableCollection<MealsModel> meals, string kitchen_id, string kitchen_zipcode, string available_zipcodes)
        {
            InitializeComponent();
            availableZipcode = available_zipcodes;
            kitchenZipcode = kitchen_zipcode;
            Console.WriteLine("KitchenZipcodeIS"+ kitchenZipcode);
            mealsOrdered = meals;
            SetupUI();

            Auto_Fill("user_id", fullNameField);
            Auto_Fill("email", emailField);
            Auto_Fill("phone", phoneField);
            Auto_Fill("street", streetAddressField);
            Auto_Fill("city", cityField);
            Auto_Fill("state", stateField);
            Auto_Fill("zip", zipCodeField);
            Auto_Fill("addressUnit", addressUnitField);
            Auto_Fill("deliveryInstruction", deliveryInstructionField);

            currentOrder.kitchen_id = kitchen_id;

            foreach (var meal in mealsOrdered)
            {
                currentOrder.ordered_items.Add(new Dictionary<string, string>()
                {
                    { "meal_id", meal.id },
                    { "qty", meal.order_qty.ToString() },
                    { "name", meal.title },
                    { "price", meal.price }
                });

                if (!meal.order_qty.ToString().Equals("0"))
                {
                    addOrderItems(meal.id, meal.order_qty.ToString(), meal.title, meal.price, true);
                }
            }

            result += "]";
            Console.WriteLine("This will be the result going into items: " + result);
        }

        // HELPER FUNCTION 1
        // THIS FUNCTION ADD ITEMS TO AN ORDER 
        private void addOrderItems(string v1, string v2,string v3, string v4, bool last)
        {
            string item = "";

            if (calls == 0)
            {
                item = "{\"item_uid\": " + "\"" + v1 + "\"," + "\"qty\": " + "\"" + v2 + "\"," + "\"name\": " + "\"" + v3 + "\"," + "\"price\": " + "\"" + v4 + "\"}";
                result += "[" + item;
                calls++;
            }
            else
            {
                item = ",{\"item_uid\": " + "\"" + v1 + "\"," + "\"qty\": " + "\"" + v2 + "\"," + "\"name\": " + "\"" + v3 + "\"," + "\"price\": " + "\"" + v4 + "\"}";
                result += item;
            }
        }

        void SetupUI()
        {

            var mainStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };

            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });

            var grid1 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 10 };
            var grid2 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 10 };
            var grid3 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = new Thickness(0, 10, 0, 0) };
            var grid4 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 50, Margin = new Thickness(0, 0, 0, 10) };
            var grid5 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 50, Margin = 10 };
            var grid6 = new StackLayout() { Orientation = StackOrientation.Horizontal, Margin = 10 };


            var addressFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };
            var couponFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };
            var creditCardFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };

            var orderTitleLabel = new Label() { Text = "Your Order:" };


            var orderNamesStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand};
            var orderPriceAndQtyLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.EndAndExpand };

            //foreach (var meal in mealsOrdered)
            //{
            //    // calculate total cost for the order
            //    totalCostsForMeals += (double.Parse(meal.price) * meal.order_qty);

            //    // create label for the UI
            //    if (meal.order_qty >= 1)
            //    {
            //        var mealNameLabel = new Label() { Text = meal.title, FontSize = 12, Margin = new Thickness(0, 0, 15, 0) };
            //        var mealPriceAndQty = new Label() { Text = meal.order_qty.ToString() + "  x  " + "$ " + FormatCurrency(meal.price), FontSize = 12, WidthRequest = 200 };
            //        orderNamesStackLayout.Children.Add(mealNameLabel);
            //        orderPriceAndQtyLayout.Children.Add(mealPriceAndQty);
            //    }
            //}

            var orderStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.StartAndExpand };
            foreach (var meal in mealsOrdered)
            {
                // calculate total cost for the order
                totalCostsForMeals += (double.Parse(meal.price) * meal.order_qty);

                // create label for the UI
                if (meal.order_qty >= 1)
                {
                    Grid orderItemGrid = new Grid
                    {
                        RowDefinitions =
                        {
                            new RowDefinition()
                        },
                        ColumnDefinitions =
                        {
                            new ColumnDefinition()
                        }
                    };
                    var mealNameLabel = new Label() { Text = meal.title, FontSize = 12, Margin = new Thickness(0, 0, 0, 0) };
                    var mealPriceAndQty = new Label() { Text = meal.order_qty.ToString() + "  x  " + "$ " + FormatCurrency(meal.price), FontSize = 12, HorizontalOptions = LayoutOptions.EndAndExpand };
                    orderItemGrid.Children.Add(mealNameLabel);
                    orderItemGrid.Children.Add(mealPriceAndQty,1,0);
                    orderStackLayout.Children.Add(orderItemGrid);
                }
            }
            //Console.WriteLine(couponInfo.couponCredit);

            /*var couponLabel = new Label() { Text = String.Concat("Coupon ", couponID), FontSize = 12, BindingContext = couponID, };
            var couponAmount = new Label() { Text = "$ " + FormatCurrency(couponCredit.ToString()), FontSize = 12, HorizontalTextAlignment = TextAlignment.End, BindingContext = couponCredit };*/


            orderNamesStackLayout.Children.Add(couponLabel);
            orderPriceAndQtyLayout.Children.Add(couponAmount);

            

            // Add delivery charges
            double delCharges = 5.00;

            // calculate tax amount
            //calculatedTaxAmount = Math.Round((totalCostsForMeals * 0.09), 2);
            
            // tax rate
            //calculatedTaxAmount = Math.Round((totalCostsForMeals * 0.09), 2);
            calculatedTaxAmount = Math.Round((totalCostsForMeals * 0.00), 2);
            totalCostsForMeals += delCharges;
            
            String currencyTax = FormatCurrency(calculatedTaxAmount.ToString());
            var taxLabel = new Label() { Text = "Tax", FontSize = 12 };
            //var deliveryLabel = new Label() { Text = "Delivery charges", FontSize = 12 };

            var taxAmount = new Label() { Text = "$ " + currencyTax, FontSize = 12, HorizontalTextAlignment = TextAlignment.End };
            //var deliveryAmount = new Label() { Text = "$ " + FormatCurrency(delCharges.ToString()), FontSize = 12, HorizontalTextAlignment = TextAlignment.End };

            totalAmountTextLabel.Text = "Total Amount";
            totalAmountLabel.Text = "$ " + FormatCurrency((calculatedTaxAmount + totalCostsForMeals).ToString());
            /*var totalAmountTextLabel = new Label() { Text = "Total Amount", FontSize = 14, FontAttributes = FontAttributes.Bold };
            var totalAmountLabel = new Label() { Text = "$ " + FormatCurrency((calculatedTaxAmount + totalCostsForMeals+ couponCredit).ToString()), FontSize = 14, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.End };*/

            // store total amount of the order
            currentOrder.totalAmount = (calculatedTaxAmount + totalCostsForMeals).ToString();

            Guid g;
            // Create and display the value of two GUIDs.
            g = Guid.NewGuid();
            currentOrder.order_id = g.ToString();

            
            orderNamesStackLayout.Children.Add(taxLabel);
            orderNamesStackLayout.Children.Add(deliveryLabel);
            orderNamesStackLayout.Children.Add(totalAmountTextLabel);
            

            orderPriceAndQtyLayout.Children.Add(taxAmount);
            orderPriceAndQtyLayout.Children.Add(deliveryAmount);
            orderPriceAndQtyLayout.Children.Add(totalAmountLabel);


            //grid1.Children.Add(firstNameField);
            //grid1.Children.Add(lastNameField);
            grid1.Children.Add(fullNameField);
            grid1.Children.Add(emailField);
            grid1.Children.Add(phoneField);
            addressFieldStackLayout.Children.Add(cityField);
            addressFieldStackLayout.Children.Add(stateField);

//             addressFieldStackLayout.Children.Add(zipCodeField);
//             addressFieldStackLayout.Children.Add(couponCodeField);
//             applyCouponButton.Clicked += Handle_Apply_Coupon_Clicked();
//             addressFieldStackLayout.Children.Add(applyCouponButton);

            addressFieldStackLayout.Children.Add(zipCodeField);
            couponFieldStackLayout.Children.Add(couponCodeField);

            
            applyCouponButton.Clicked += Handle_Apply_Coupon_Clicked();
            couponFieldStackLayout.Children.Add(applyCouponButton);

            Label creditCardLabel = new Label() { Text = "Please fill in the credit card information if you want to checkout with credit card" };
            creditCardFieldStackLayout.Children.Add(CreditCardNumberField);
            creditCardFieldStackLayout.Children.Add(ExpMonthField);
            creditCardFieldStackLayout.Children.Add(ExpYearField);
            creditCardFieldStackLayout.Children.Add(CVCField);
            grid2.Children.Add(streetAddressField);
            grid2.Children.Add(addressUnitField);
            
            grid2.Children.Add(addressFieldStackLayout);
            grid2.Children.Add(deliveryInstructionField);
            grid2.Children.Add(couponFieldStackLayout);

            //grid3.Children.Add(creditCardLabel);
            //grid3.Children.Add(creditCardFieldStackLayout);

            //grid4.Children.Add(orderTitleLabel);

            grid5.Children.Add(orderTitleLabel);
            grid5.Children.Add(orderStackLayout);
            grid6.Children.Add(orderNamesStackLayout);
            grid6.Children.Add(orderPriceAndQtyLayout);


            grid.Children.Add(grid1);
            grid.Children.Add(grid2);
            grid.Children.Add(grid3);
            //grid.Children.Add(grid4);
            grid.Children.Add(grid5);
            grid.Children.Add(grid6);

            Grid.SetRow(grid1, 0);
            Grid.SetRow(grid2, 1);
            Grid.SetRow(grid3, 2);
            //Grid.SetRow(grid4, 3);
            Grid.SetRow(grid5, 3);
            Grid.SetRow(grid6, 4);


            var scrollView = new ScrollView()
            {
                Content = grid
            };

            var checkoutStripeButton = new Button() { Text = "Checkout with Credit Card", HeightRequest = 40, Margin = new Thickness(20, 10, 20, 10), BorderWidth = 0.5, BorderColor = Color.Gray};
            checkoutStripeButton.Clicked += Handle_Clicked();

            var checkoutPaypalButton = new Button() { Text = "Checkout with Paypal", HeightRequest = 40, Margin = new Thickness(20, 10, 20, 10), BorderWidth = 0.5, BorderColor = Color.Gray, CornerRadius = 10, BackgroundColor = Color.Transparent, TextColor = Color.FromHex("#a0050f") };
            checkoutPaypalButton.Clicked += Handle_Clicked();

            mainStackLayout.Children.Add(scrollView);
            //mainStackLayout.Children.Add(checkoutStripeButton);
            mainStackLayout.Children.Add(checkoutPaypalButton);

            var copyrightLabel = new Label() { Text = "© Infinite Options v1.8", FontSize = 10, HorizontalOptions = LayoutOptions.Center,  Margin = new Thickness(20, 10, 20, 10) };

            mainStackLayout.Children.Add(copyrightLabel);

            Content = new StackLayout
            {
                Children = { mainStackLayout }
            };
        }

        private EventHandler Handle_Clicked()
        {
            return placeOrder;
        }
        private EventHandler Handle_Apply_Coupon_Clicked()
        {
            return applyCoupon;
        }

        // COUPONS FUNCTION UPDATED BY CARLOS 
        async void applyCoupon(object sender, System.EventArgs e)
        {
            // Console.WriteLine("couponCodeField.Text: " + couponCodeField.Text);

            string couponCode = couponCodeField.Text.ToString().Trim();
            var client = new HttpClient();
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/couponDetails/" + couponCode);
            request.Method = HttpMethod.Get;

            HttpResponseMessage response = await client.SendAsync(request);

            // COUPON SEARCH SUCCESSFUL 
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string data = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine("This is the data from the coupon = " + data);
                ServingFreshCoupon coupon = new ServingFreshCoupon();
                coupon = JsonConvert.DeserializeObject<ServingFreshCoupon>(data);

                // IF COUPON IS ACTIVE, THEN WE PROCESS IT (SECURITY LAYER 1)
                // NEED TO PUT A TRY STATEMENT TO MAKE SURE THE ARRAY OF COUPON IS NOT EMPTY
                if (coupon.result.result[0].valid.Equals("TRUE") && coupon.result.result[0].num_used < coupon.result.result[0].limits)
                {
                    // IF EMAIL_ID MATCH THE COUPON EMAIL_ID (SECURITY LAYER 2)
                    if (coupon.result.result[0].email_id.Equals(emailField.Text))
                    {
                        await DisplayAlert("Coupon code is applied", coupon.result.result[0].notes, "OK");
                        InitializeComponent();

                        double c_amount = 0.0;

                        if (coupon.result.result[0].discount_percent != 0)
                        {
                            // DISCOUNT_PERCENT COUPON
                            double percent = coupon.result.result[0].discount_percent / 100;

                            // MINUS DELIVERY FEE WHICH IS $ 5.00
                            c_amount = (totalCostsForMeals - 5) * percent;
                            couponAmount.Text = "$ " + String.Format("{0:0.00;}", c_amount);
                        }

                        if (coupon.result.result[0].discount_amount != 0)
                        {
                            // DISCOUNT_AMOUNT COUPON
                            c_amount = coupon.result.result[0].discount_amount;
                            couponAmount.Text = "$ " + String.Format("{0:0.00;}", c_amount);
                        }

                        if (coupon.result.result[0].discount_shipping != 0)
                        {
                            // DISCOUNT_SHIPPING AMOUNT
                            c_amount = coupon.result.result[0].discount_shipping;
                            couponAmount.Text = "$ " + String.Format("{0:0.00;}", c_amount);
                            double currentDeliveryAmount = Convert.ToDouble(deliveryAmount.Text.Substring(2)) - c_amount;
                            deliveryAmount.Text = "$ " + String.Format("{0:0.00;}", currentDeliveryAmount);
                        }

                        couponObject.coupon_uid = coupon.result.result[0].coupon_uid;
                        couponObject.num_used = coupon.result.result[0].num_used + 1;

                        var newTotalPrice = (calculatedTaxAmount + totalCostsForMeals - c_amount);
                        if (newTotalPrice <= 0)
                        {
                            newTotalPrice = 0.01;
                        }

                        // U/I FIELD 
                        totalAmountLabel.Text = "$ " + FormatCurrency(newTotalPrice.ToString());

                        // ORDER FIELD 
                        currentOrder.totalAmount = newTotalPrice.ToString();

                        ((Button)sender).IsEnabled = false;
                    }
                    else
                    {
                        await DisplayAlert("Alert!", "Your email doesn't match our records. Please provide the email associated with this coupon", "OK");
                    }
                }
                else
                {
                    couponCodeField.Text = null;
                    await DisplayAlert("Alert!", "Your coupon is no longer valid", "OK");
                }
            }
            else
            {   // NOT ABLE TO FIND THE COUPON INFORMATION OR USER WROTE THE COUPON CODE INCORRECTLY
                await DisplayAlert("Alert!", "We weren't able to locate your coupon information. Please reenter your coupon code as provided.", "OK");
            }
        }

        async void placeOrder(object sender, System.EventArgs e)
        {
            var button = (Button)sender;
            var paymentMethod = button.Text;

            Console.WriteLine("fullNameField.Text: " + fullNameField.Text);
            Console.WriteLine("emailField.Text: " + emailField.Text);
            Console.WriteLine("phoneField.Text: " + phoneField.Text);
            Console.WriteLine("streetAddressField.Text: " + streetAddressField.Text);
            Console.WriteLine("cityField.Text: " + cityField.Text);
            Console.WriteLine("stateField.Text: " + stateField.Text);
            Console.WriteLine("zipCodeField.Text: " + zipCodeField.Text);


            if (emailField.Text != null && !IsValidEmail(emailField.Text))
            {
                await DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }


            if (fullNameField.Text.Length == 0 || emailField.Text.Length == 0 || phoneField.Text.Length == 0 || streetAddressField.Text.Length == 0 || cityField.Text.Length == 0 || stateField.Text.Length == 0 || zipCodeField.Text.Length == 0)
            {
                await DisplayAlert("Error", "Please enter all information within the boxes provided.", "OK");
                return;
            }

            if (fullNameField.Text != null)
            {
                Application.Current.Properties["user_id"] = fullNameField.Text;
                currentOrder.name = fullNameField.Text;
            }

            if (emailField.Text != null)
            {
                Application.Current.Properties["email"] = emailField.Text;
                currentOrder.email = emailField.Text;
            }

            if (phoneField.Text != null)
            {
                Application.Current.Properties["phone"] = phoneField.Text;
                currentOrder.phone = phoneField.Text;
            }

            if (streetAddressField.Text != null)
            {
                streetAddressField.Text = streetAddressField.Text.Trim();
                Application.Current.Properties["street"] = streetAddressField.Text;
                currentOrder.street = streetAddressField.Text;
            }
            if (cityField.Text != null)
            {
                cityField.Text = cityField.Text.Trim();
                Application.Current.Properties["city"] = cityField.Text;
                currentOrder.city = cityField.Text;
            }
            if (stateField.Text != null)
            {
                stateField.Text = stateField.Text.Trim();
                Application.Current.Properties["state"] = stateField.Text;
                currentOrder.state = stateField.Text;
            }
            if (addressUnitField.Text != null)
            {
                if (addressUnitField.Text.Length != 0)
                {
                    addressUnitField.Text.Trim();
                    Application.Current.Properties["addressUnit"] = addressUnitField.Text;
                    currentOrder.address_unit = addressUnitField.Text;
                }
                else
                {
                    Application.Current.Properties["addressUnit"] = "";
                    currentOrder.address_unit = "";
                }
            }
            if (deliveryInstructionField.Text != null)
            {
                if (deliveryInstructionField.Text.Length != 0)
                {
                    Application.Current.Properties["deliveryInstruction"] = deliveryInstructionField.Text;
                    currentOrder.delivery_instructions = deliveryInstructionField.Text;
                }
                else
                {
                    Application.Current.Properties["deliveryInstruction"] = "";
                    currentOrder.delivery_instructions = "";
                }
            }

            if (zipCodeField.Text != null)
            {
                /*List<String> ZipCodes = new List<String>()
                {
                    "94024",
                    "94087",
                    "95014",
                    "95030",
                    "95032",
                    "95051",
                    "95070",
                    "95111",
                    "95112",
                    "95120",
                    "95123",
                    "95124",
                    "95125",
                    "95129",
                    "95130",
                    "95128",
                    "95122",
                    "95118",
                    "95126",
                    "95136",
                    "95113",
                    "95117"
                };*/

                string[] available_zipcode_array = availableZipcode.Split(',');
                bool is_available = false;
                foreach(var zipcode in available_zipcode_array)
                {
                    if(zipCodeField.Text.Trim() == zipcode)
                    {
                        is_available = true;
                        break;
                    }
                }
                if (is_available)
                {
                    Application.Current.Properties["zip"] = zipCodeField.Text;
                    currentOrder.zipCode = zipCodeField.Text;
                }
                else
                {
                    await DisplayAlert("Sorry for the inconvience!", "Serving Now is only accepting orders from the " +
availableZipcode+" zipcodes.", "OK");
                    return;
                }
                //Old zipcode validation
                //List<String> zipcodesnew = new List<String>();

                //var request_zipcodes = new HttpRequestMessage();
                ////request_zipcodes.RequestUri = new Uri("http://10.0.2.2:5000/api/v1/zipcodes");
                //request_zipcodes.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/zipcodes");
                ////request.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/coupons");
                //request_zipcodes.Method = HttpMethod.Get;
                //var client_zipcodes = new HttpClient();
                //HttpResponseMessage response_zipcodes = await client_zipcodes.SendAsync(request_zipcodes);

                //Console.WriteLine("Line305");
                //if (response_zipcodes.StatusCode == System.Net.HttpStatusCode.OK)
                //{
                //    HttpContent content_zip_codes = response_zipcodes.Content;
                //    var zipcodesString = await content_zip_codes.ReadAsStringAsync();
                //    JObject zipCodesJson = JObject.Parse(zipcodesString);

                //    foreach (var k in zipCodesJson["result"]["zipcodes"])
                //    {
                //        zipcodesnew.Add((string)k);
                //    }
                //}
                //if (zipcodesnew.Contains(zipCodeField.Text.Trim()))
                //{
                //    Console.WriteLine("customer zipcode: " + zipCodeField.Text.Trim());
                //    Console.WriteLine("farmer zipcode: " + kitchenZipcode.Trim());
                //    /*if (zipCodeField.Text.Trim() != parseAreaToZipcode(kitchenZipcode.Trim()))
                //    {
                //        await DisplayAlert("Sorry for the inconvience!", "Serving Now is only accepting orders from farms within " + formatZipcode(zipCodeField.Text.Trim()) + ".", "OK");
                //        return;
                //    }*/
                //    Application.Current.Properties["zip"] = zipCodeField.Text;
                //    currentOrder.zipCode = zipCodeField.Text;
                //}
                //else
                //{
                //    await DisplayAlert("Sorry for the inconvience!", "Serving Now is only accepting orders from the" +
                //        " 94024, 94087, 95014, 95030, 95032, 95051, 95052, 95070, 95111, 95112, 95113, 95117, 95118, 95120, 95123, " +
                //        "95124,95125, 95126, 95128, 95129, 95130 and 95136  zip codes.", "OK");
                //    return;
                //}
            }

            //((Button)sender).IsEnabled = false;
            XDocument requestDoc = new XDocument(
                new XElement("AddressValidateRequest",
                new XAttribute("USERID", "400INFIN1745"),
                new XElement("Revision", "1"),
                new XElement("Address",
                new XAttribute("ID", "0"),
                new XElement("Address1", streetAddressField.Text),
                new XElement("Address2", addressUnitField.Text),
                new XElement("City", cityField.Text),
                new XElement("State", stateField.Text),
                new XElement("Zip5", zipCodeField.Text),
                new XElement("Zip4", "")
                     )
                 )
             );
            var url = "http://production.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + requestDoc;
            Console.WriteLine(url);
            var addressClient = new WebClient();
            var addressResponse = addressClient.DownloadString(url);

            var xdoc = XDocument.Parse(addressResponse.ToString());
            Console.WriteLine(xdoc);
            string latitude = "0";
            string longtitude = "0";
            foreach (XElement element in xdoc.Descendants("Address"))
            {
                if (GetXMLElement(element, "Error").Equals(""))
                {
                    if (GetXMLElement(element, "DPVConfirmation").Equals("Y") && GetXMLElement(element, "Zip5").Equals(zipCodeField.Text) && GetXMLElement(element, "City").Equals(cityField.Text.ToUpper())) // Best case
                    {
                        // Get longitude and latitide because we can make a deliver here. Move on to next page.
                        // Console.WriteLine("The address you entered is valid and deliverable by USPS. We are going to get its latitude & longitude");
                        //GetAddressLatitudeLongitude();
                        Geocoder geoCoder = new Geocoder();

                        IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(streetAddressField.Text + "," + cityField.Text + "," + stateField.Text);
                        Position position = approximateLocations.FirstOrDefault();

                        latitude = $"{position.Latitude}";
                        longtitude = $"{position.Longitude}";
                        break;
                    }
                    else if (GetXMLElement(element, "DPVConfirmation").Equals("D"))
                    {
                        //await DisplayAlert("Alert!", "Address is missing information like 'Apartment number'.", "Ok");
                        //return;
                    }
                    else
                    {
                        //await DisplayAlert("Alert!", "Seems like your address is invalid.", "Ok");
                        //return;
                    }


                }
                else
                {   // USPS sents an error saying address not found in there records. In other words, this address is not valid because it does not exits.
                    //Console.WriteLine("Seems like your address is invalid.");
                    //await DisplayAlert("Alert!", "Error from USPS. The address you entered was not found.", "Ok");
                    //return;
                }
            }
            if (latitude == "0" || longtitude == "0")
            {
                await DisplayAlert("Alert!", "Seems like your address is invalid.", "Ok");
                return;
            }
            currentOrder.addressLatitude = latitude;
            currentOrder.addressLongitude = longtitude;
            currentOrder.appVersion = CrossLatestVersion.Current.InstalledVersionNumber;
            await Application.Current.SavePropertiesAsync();
            //currentOrder.deliveryTime = deliveryTime.Time.ToString();
            UserDialogs.Instance.ShowLoading("Sending your request...");

            if (paymentMethod == "Checkout with Credit Card")
            {
                if (CreditCardNumberField.Text == null || ExpMonthField.Text == null || ExpYearField.Text == null || CVCField.Text == null)
                {
                    await DisplayAlert("Error", "Please fill out the credit card information if you want to checkout with credit card.", "OK");
                    return;
                    //if (CreditCardNumberField.Text.Length == 0 || ExpMonthField.Text.Length == 0 || ExpYearField.Text.Length == 0 || CVCField.Text.Length == 0)
                    //{
                    //    await DisplayAlert("Error", "Please fill out the credit card information if you want to checkout with credit card.", "OK");
                    //    return;
                    //}
                }
                else
                {
                    var request = new HttpRequestMessage();
                    //request.RequestUri = new Uri("http://10.0.2.2:5000/api/v1/payment");
                    request.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/payment");
    

                    request.Method = HttpMethod.Get;
                    var client = new HttpClient();
                    MultipartFormDataContent requestContent = new MultipartFormDataContent();
                    StringContent amountContent = new StringContent((float.Parse(currentOrder.totalAmount) * 100).ToString(), Encoding.UTF8);
                    requestContent.Add(amountContent, "amount");
                    request.Content = requestContent;


                    HttpResponseMessage response = await client.SendAsync(request);
                    //Howard test key
                    //StripeConfiguration.ApiKey = "pk_test_j5YImiDFgybfafp8HuEkn6Ou00JtFKI0s9";
                    //Prashant test key
                    //StripeConfiguration.ApiKey = "pk_test_6RSoSd9tJgB2fN2hGkEDHCXp00MQdrK3Tw";
                    //live key
                    StripeConfiguration.ApiKey = "pk_live_g0VCt4AW6k7tyjRw61O3ac5a00Tefdbp8E";
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        HttpContent content = response.Content;
                        var responseString = await content.ReadAsStringAsync();
                        var messageObj = JsonConvert.DeserializeObject<message>(responseString);
                        var client_secret = messageObj.client_secret;

                        var service = new PaymentIntentService();
                        var getOptions = new PaymentIntentGetOptions
                        {
                            ClientSecret = client_secret
                        };
                        PaymentIntent paymentIntent = service.Get(messageObj.id, getOptions);

                        var paymentMethodCardCreateOption = new PaymentMethodCardCreateOptions
                        {
                            Number = CreditCardNumberField.Text,
                            Cvc = CVCField.Text,
                            ExpMonth = long.Parse(ExpMonthField.Text),
                            ExpYear = long.Parse(ExpYearField.Text),
                        };

                        var paymentMethodDataOption = new PaymentIntentPaymentMethodDataOptions
                        {
                            Card = paymentMethodCardCreateOption,
                            Type = "card",

                        };
                        List<String> methodTypes = new List<String>();
                        methodTypes.Add("pm_card_visa");
                        var confirmOptions = new PaymentIntentConfirmOptions
                        {
                            //PaymentMethod = "pm_card_visa",
                            PaymentMethodData = paymentMethodDataOption,
                            ReceiptEmail = emailField.Text,
                            ClientSecret = client_secret,
                        };
                        try
                        {
                            var status = service.Confirm(messageObj.id, confirmOptions);
                            if (status.Status == "succeeded")
                            {
                                await sendOrderRequest(currentOrder);
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Thank you!", "Your order has been placed." + System.Environment.NewLine + "An email receipt has been sent to " + currentOrder.email + ".", "OK");
                                
                            }
                            else
                            {
                                UserDialogs.Instance.HideLoading();
                                await DisplayAlert("Failed", "Please varify your credit card infomation, or try another card. You can also try using Paypal to checkout.", "OK");
                                return;
                            }
                        }
                        catch (Exception)
                        {
                            UserDialogs.Instance.HideLoading();
                            await DisplayAlert("Failed", "Please varify your credit card infomation, or try another card. You can also try using Paypal to checkout.", "OK");
                            return;
                        }
                    }
                }
            }
            else {
                var guid = Preferences.Get("guid", null);
                if (guid != null)
                {
                    currentOrder.notification_enabled = true;
                }
                await sendOrderRequest(currentOrder);
                await tagUser(currentOrder);
                UserDialogs.Instance.HideLoading();
            }

            if (couponCodeField.Text != null)
            {
                Console.WriteLine("We have applied coupon");
                var couponString = JsonConvert.SerializeObject(couponObject);
                var coupondMessage = new StringContent(couponString, Encoding.UTF8, "application/json");
                var httpClient = new HttpClient();
                var request = new HttpRequestMessage();

                request.Method = HttpMethod.Post;
                request.Content = coupondMessage;
                var httpResponse = await httpClient.PostAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/couponDetails", coupondMessage);

                if (httpResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine("Coupons-updated");
                    /*HttpContent content_put = response_put.Content;
                    var couponsString_put = await content_put.ReadAsStringAsync();*/
                }
            }
            if (paymentMethod == "Checkout with Paypal") {
                await DisplayAlert("Thank you!", "Your order has been placed." + System.Environment.NewLine + "An email receipt has been sent to " + currentOrder.email + ". Please complete the payment process by clicking the button below.", "Continue to PayPal");
                Device.OpenUri(new System.Uri("https://servingnow.me/payment/" + currentOrder.order_id + "/" + currentOrder.totalAmount));
            }

            // await Navigation.PopModalAsync();
            await Navigation.PopAsync();
            // "(Copyright Symbol) 2019 Infinite Options   v1.2"
        }

        // HELPER FUNCTION 2
        // NOTE THIS FUNCTION NEEDS TO BE PARSE THE USER NAME INTO
        // FIRST NAME AND LAST NAME
        private void parseName(string name)
        {

        }

        bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        void Auto_Fill(string key, Entry location)
        {
            if (Application.Current.Properties.ContainsKey(key) && Application.Current.Properties[key] != null)
            {
                location.Text = Application.Current.Properties[key].ToString();
            }
        }

        // SEND ORDER REQUEST FUNCTION UPDATED BY CARLOS 
        async Task sendOrderRequest(OrderInfo currentOrder)
        {

            OrderObject order = new OrderObject();
            // PURCHASE TABLE SCHEMA
            order.customer_uid = currentOrder.phone;
            order.business_uid = currentOrder.kitchen_id;
            order.delivery_first_name = currentOrder.name; // full name
            order.delivery_last_name = "";
            order.delivery_email = currentOrder.email;
            order.delivery_phone = currentOrder.phone;
            order.delivery_address = currentOrder.street;
            order.delivery_unit = currentOrder.address_unit;
            order.delivery_city = currentOrder.city;
            order.delivery_state = currentOrder.state;
            order.delivery_zip = currentOrder.zipCode;
            order.delivery_instructions = currentOrder.delivery_instructions;
            order.delivery_longitude = currentOrder.addressLongitude;
            order.delivery_latitude = currentOrder.addressLatitude;
            order.items = result;
            order.order_instructions = "empty";
            order.purchase_notes = "empty";
            order.amount_due = currentOrder.totalAmount;
            order.amount_discount = "empty";
            order.amount_paid = "empty";

            // PAYMENTS TABLE SCHEMA
            order.paid = currentOrder.paid;
            order.notification_enabled = currentOrder.notification_enabled;
            order.paymentType = currentOrder.paymentType;
            order.appVersion = currentOrder.appVersion;

            var data = JsonConvert.SerializeObject(order);
            var content = new StringContent(data, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var request = new HttpRequestMessage();
                request.Method = HttpMethod.Post;
                request.Content = content;
                var httpResponse = await httpClient.PostAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/purchaseData", content);
                Console.WriteLine("This is the order's response " + httpResponse);
            }
        }

        async Task tagUser(OrderInfo currentOrder)
        {
            var guid = Preferences.Get("guid", null);
            if(guid == null)
            {
                return;
            }
            var tags = "email_" + currentOrder.email + "," + "zip_" + currentOrder.zipCode + "," + "farm_" + currentOrder.kitchen_id;
            
            MultipartFormDataContent updateRegistrationInfoContent = new MultipartFormDataContent();
            StringContent guidContent = new StringContent(guid, Encoding.UTF8);
            StringContent tagsContent = new StringContent(tags, Encoding.UTF8);
            updateRegistrationInfoContent.Add(guidContent, "guid");
            updateRegistrationInfoContent.Add(tagsContent, "tags");

            var updateRegistrationRequest = new HttpRequestMessage();
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    updateRegistrationRequest.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/update_registration_guid_iOS");
                    //updateRegistrationRequest.RequestUri = new Uri("http://10.0.2.2:5000/api/v1/update_registration_guid_iOS");
                    break;
                case Device.Android:
                    updateRegistrationRequest.RequestUri = new Uri("https://phaqvwjbw6.execute-api.us-west-1.amazonaws.com/dev/api/v1/update_registration_guid_android");
                    //updateRegistrationRequest.RequestUri = new Uri("http://10.0.2.2:5000/api/v1/update_registration_guid_android");
                    break;
            }
            updateRegistrationRequest.Method = HttpMethod.Post;
            updateRegistrationRequest.Content = updateRegistrationInfoContent;
            var updateRegistrationClient = new HttpClient();
            HttpResponseMessage updateRegistrationResponse = await updateRegistrationClient.SendAsync(updateRegistrationRequest);
        }

        private String FormatCurrency(String number)
        {
            if (!number.Contains("."))
            {
                return number + ".00";
            }
            if (number[number.Length - 2] == '.')
            {
                return number + "0";
            }
            return number;
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

        private string parseAreaToZipcode(string zipcode)
        {
            if (zipcode == "Almaden")
            {
                return "95120";
            }
            if (zipcode == "Evergreen")
            {
                return "95135";
            }
            if (zipcode == "Santa Cruz")
            {
                return "95060";
            }
            if (zipcode == "Other")
            {
                return "90000";
            }
            return "";
        }

        public static string GetXMLElement(XElement element, string name)
        {
            var el = element.Element(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }

        public static string GetXMLAttribute(XElement element, string name)
        {
            var el = element.Attribute(name);
            if (el != null)
            {
                return el.Value;
            }
            return "";
        }
    }
}
