using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using InfiniteMeals.Kitchens.Controller;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace InfiniteMeals.PromptAddress
{
    public partial class PromptAddressPage : ContentPage
    {
        public Image servingNowLogo = new Image { Source = "ServingNowLogo.png", HorizontalOptions = LayoutOptions.Start };
        public Label welcomeLabel = new Label() { Text = "Welcome to Serving Now", FontSize = 30, FontAttributes = FontAttributes.Bold, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.EndAndExpand};
        public Label promptLabel = new Label() { Text = "Please enter your delivery address to display your delivery days." };

        public Entry fullNameField = new Entry() { Placeholder = "Full name", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };
        public Entry emailField = new Entry() { Placeholder = "Email", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Email };
        public Entry phoneField = new Entry() { Placeholder = "Phone", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, Keyboard = Keyboard.Numeric };
        public Entry streetAddressField = new Entry() { Placeholder = "Street Address", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };
        public Entry cityField = new Entry() { Placeholder = "City", HorizontalOptions = LayoutOptions.FillAndExpand, VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45 };

        public Entry stateField = new Entry() { Placeholder = "State", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 100 };
        public Entry zipCodeField = new Entry() { Placeholder = "Zip Code", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 170, Keyboard = Keyboard.Numeric };
        public Entry addressUnitField = new Entry() { Placeholder = "Address Unit", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 170};
        public Entry deliveryInstructionField = new Entry() { Placeholder = "Delivery Instruction (e.g. Gate Code, etc)", VerticalOptions = LayoutOptions.Center, HorizontalTextAlignment = TextAlignment.Start, HeightRequest = 45, MinimumWidthRequest = 170 };
        public PromptAddressPage()
        {
            InitializeComponent();
            SetupUI();

            //Auto_Fill("street", streetAddressField);
            //Auto_Fill("city", cityField);
            //Auto_Fill("state", stateField);
            //Auto_Fill("zip", zipCodeField);
            //Auto_Fill("addressUnit", addressUnitField);

            Auto_Fill("user_id", fullNameField);
            Auto_Fill("email", emailField);
            Auto_Fill("phone", phoneField);
            Auto_Fill("street", streetAddressField);
            Auto_Fill("city", cityField);
            Auto_Fill("state", stateField);
            Auto_Fill("zip", zipCodeField);
            Auto_Fill("addressUnit", addressUnitField);
            Auto_Fill("deliveryInstruction", deliveryInstructionField);
        }

        void SetupUI()
        {
            var mainStackLayout = new StackLayout() { Orientation = StackOrientation.Vertical };
            var grid = new Grid();
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            var nameFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };
            nameFieldStackLayout.Children.Add(fullNameField);
            var addressFieldStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand };
            addressFieldStackLayout.Children.Add(cityField);
            addressFieldStackLayout.Children.Add(stateField);
            addressFieldStackLayout.Children.Add(zipCodeField);
            var welcomeLabelStackLayout = new StackLayout() { Orientation = StackOrientation.Horizontal, MinimumHeightRequest = 45, HorizontalOptions = LayoutOptions.FillAndExpand, Spacing = 45};
            welcomeLabelStackLayout.Children.Add(servingNowLogo);
            welcomeLabelStackLayout.Children.Add(welcomeLabel);
            var grid1 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 10 };
            grid1.Children.Add(welcomeLabelStackLayout);
            grid1.Children.Add(promptLabel);
            grid1.Children.Add(nameFieldStackLayout);
            grid1.Children.Add(emailField);
            grid1.Children.Add(phoneField);
            var grid2 = new StackLayout() { Orientation = StackOrientation.Vertical, MinimumHeightRequest = 150, Margin = 10 };
            grid2.Children.Add(streetAddressField);
            grid2.Children.Add(addressUnitField);
            grid2.Children.Add(addressFieldStackLayout);
            grid2.Children.Add(deliveryInstructionField);
            grid.Children.Add(grid1);
            grid.Children.Add(grid2);

            Grid.SetRow(grid1, 0);
            Grid.SetRow(grid2, 1);

            var scrollView = new ScrollView()
            {
                Content = grid
            };
            var saveButton = new Button() { Text = "Save", HeightRequest = 40, Margin = new Thickness(20, 10, 20, 10), BorderWidth = 0.5, BorderColor = Color.Gray, TextColor = Color.FromHex("#a0050f"), CornerRadius = 10, BackgroundColor = Color.Transparent };
            saveButton.Clicked += Handle_Clicked();

            var skipButton = new Button() { Text = "Skip", HeightRequest = 40, Margin = new Thickness(20, 0, 20, 0), BorderWidth = 0.5, BorderColor = Color.Gray, TextColor = Color.FromHex("#a0050f"), CornerRadius = 10, BackgroundColor = Color.Transparent };
            skipButton.Clicked += Skip_Clicked();
            mainStackLayout.Children.Add(scrollView);
            mainStackLayout.Children.Add(saveButton);
            if (!(Application.Current.Properties.ContainsKey("zip") && Application.Current.Properties["zip"] != null))
            {
                mainStackLayout.Children.Add(skipButton);
            }
            Content = new StackLayout
            {
                Children = { mainStackLayout }
            };
        }

        private EventHandler Handle_Clicked()
        {
            return saveAddress;
        }
        private EventHandler Skip_Clicked()
        {
            return skipToFarms;
        }
        void Auto_Fill(string key, Entry location)
        {
            if (Application.Current.Properties.ContainsKey(key) && Application.Current.Properties[key] != null)
            {
                location.Text = Application.Current.Properties[key].ToString();
            }
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
        async void saveAddress(object sender, System.EventArgs e)
        {
            if (emailField.Text != null && !IsValidEmail(emailField.Text))
            {
                await DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }
            if (fullNameField.Text == null || emailField.Text == null || phoneField.Text == null || streetAddressField.Text == null || cityField.Text == null || stateField.Text == null || zipCodeField.Text == null || fullNameField.Text.Length == 0 || emailField.Text.Length == 0 || phoneField.Text.Length == 0 || streetAddressField.Text.Length == 0 || cityField.Text.Length == 0 || stateField.Text.Length == 0 || zipCodeField.Text.Length == 0)
            {
                await DisplayAlert("Error", "Please enter all information within the boxes provided.", "OK");
                return;
            }

            if (fullNameField.Text != null)
            {
                Application.Current.Properties["user_id"] = fullNameField.Text;
            }

            if (emailField.Text != null)
            {
                Application.Current.Properties["email"] = emailField.Text;
            }

            if (phoneField.Text != null)
            {
                Application.Current.Properties["phone"] = phoneField.Text;
            }
            if (streetAddressField.Text != null)
            {
                streetAddressField.Text = streetAddressField.Text.Trim();
                Application.Current.Properties["street"] = streetAddressField.Text;
            }
            if (cityField.Text != null)
            {
                cityField.Text = cityField.Text.Trim();
                Application.Current.Properties["city"] = cityField.Text;
            }
            if (stateField.Text != null)
            {
                stateField.Text = stateField.Text.Trim();
                Application.Current.Properties["state"] = stateField.Text;
            }
            if (addressUnitField.Text != null)
            {
                if (addressUnitField.Text.Length != 0)
                {
                    addressUnitField.Text = addressUnitField.Text.Trim();
                    Application.Current.Properties["addressUnit"] = addressUnitField.Text;
                }
                else
                {
                    Application.Current.Properties["addressUnit"] = "";
                }
            }
            if (zipCodeField.Text != null)
            {
                zipCodeField.Text = zipCodeField.Text.Trim();
                Application.Current.Properties["zip"] = zipCodeField.Text;
            }
            if (deliveryInstructionField.Text != null)
            {
                Application.Current.Properties["deliveryInstruction"] = deliveryInstructionField.Text;
            }
            // Setting request for USPS API
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
            var client = new WebClient();
            var response = client.DownloadString(url);

            var xdoc = XDocument.Parse(response.ToString());
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
            if(latitude == "0" || longtitude == "0")
            {
                await DisplayAlert("We couldn't find your address", "Please check for errors.", "Ok");
                return;
            }
           
            await Application.Current.SavePropertiesAsync();
            await tagUser(emailField.Text, zipCodeField.Text);
            //await DisplayAlert("Success", "Your shipping infomation has been saved", "OK");
            await Navigation.PushModalAsync(new TabbedMainPage());
        }
        async void skipToFarms(object sender, System.EventArgs e)
        {
            await Navigation.PushModalAsync(new TabbedMainPage());
        }
            private async void GetAddressLatitudeLongitude()
        {
            //Geocoder geoCoder = new Geocoder();

            //IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(streetAddressField.Text+","+cityField.Text+","+stateField.Text);
            //Position position = approximateLocations.FirstOrDefault();

            //string coordinates = $"{position.Latitude}, {position.Longitude}";
            //Console.WriteLine(coordinates);
            //await DisplayAlert("Coordinates", coordinates, "Ok");
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
        async Task tagUser(string email, string zipCode)
        {
            var guid = Preferences.Get("guid", null);
            if (guid == null)
            {
                return;
            }
            var tags = "email_" + email + "," + "zip_" + zipCode;

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
    }
}
