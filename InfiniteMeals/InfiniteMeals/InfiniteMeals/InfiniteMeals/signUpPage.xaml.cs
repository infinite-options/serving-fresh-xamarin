﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Newtonsoft.Json;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace InfiniteMeals
{
    public partial class signUpPage : ContentPage
    {
        public Boolean termsOfServiceChecked = false;
        public Boolean weeklyUpdatesChecked = false;
        public HttpClient client = new HttpClient();
        public const string signUpApi = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/signup";
        const string accountSaltURL = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/accountsalt/"; // api to get account salt; need email at the end of link
        const string loginURL = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/account/"; // api to log in; need email + hashed password at the end of link

        public class AccountSalt
        {

        }

        public class LoginResponse
        {

        }

        public class LoginPost
        {
            public string ipAddress { get; set; }
            public string browserType { get; set; }
        }

        public class SignUpResponse
        {

        }

        public class SignUpPost
        {
            public string Email { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }

        public signUpPage()
        {
            InitializeComponent();
            map.MapType = MapType.Hybrid;
            //Position position = new Position();
            //position.Latitude = 1;
            //position.Latitude = 2;
            //var mapSpan = new MapSpan(position, 100.0, 100.0);
            //map.MoveToRegion = new MapSpan();
        }

        // SignUpNewUser
        async void SignUpNewUser(object sender, System.EventArgs e)
        {
            if (userEmailAddress.Text != null && !IsValidEmail(userEmailAddress.Text))
            {
                await DisplayAlert("Error", "Please enter a valid email address.", "OK");
                return;
            }
            if (userEmailAddress.Text == null || userConfirmEmailAddress.Text == null || userPassword.Text == null || usertAddress.Text == null || userConfirmPassword.Text == null || userFirstName.Text == null || userLastName.Text == null || userLastName.Text.Length == 0 || userPhoneNumber.Text.Length == 0 || usertAddress.Text.Length == 0 || userUnitNumber.Text.Length == 0 || userCity.Text.Length == 0 || userState.Text.Length == 0 || userZipcode.Text.Length == 0)
            {
                await DisplayAlert("Error", "Please enter all information within the boxes provided.", "OK");
                return;
            }

            if (userFirstName.Text != null)
            {
                Application.Current.Properties["userFirstName"] = userFirstName.Text;
            }

            if(userLastName.Text != null)
            {
                Application.Current.Properties["userLastName"] = userLastName.Text;
            }

            if (userEmailAddress.Text != null)
            {
                Application.Current.Properties["email"] = userEmailAddress.Text;
            }

            if (userPhoneNumber.Text != null)
            {
                Application.Current.Properties["phone"] = userPhoneNumber.Text;
            }
            if (usertAddress.Text != null)
            {
                usertAddress.Text = usertAddress.Text.Trim();
                Application.Current.Properties["street"] = usertAddress.Text;
            }
            if (userCity.Text != null)
            {
                userCity.Text = userCity.Text.Trim();
                Application.Current.Properties["city"] = userCity.Text;
            }
            if (userState.Text != null)
            {
                userState.Text = userState.Text.Trim();
                Application.Current.Properties["state"] = userState.Text;
            }
            if (userUnitNumber.Text != null)
            {
                if (userUnitNumber.Text.Length != 0)
                {
                    userUnitNumber.Text = userUnitNumber.Text.Trim();
                    Application.Current.Properties["addressUnit"] = userUnitNumber.Text;
                }
                else
                {
                    Application.Current.Properties["addressUnit"] = "";
                }
            }
            if (userZipcode.Text != null)
            {
                userZipcode.Text = userZipcode.Text.Trim();
                Application.Current.Properties["zip"] = userZipcode.Text;
            }

            // Setting request for USPS API
            XDocument requestDoc = new XDocument(
                new XElement("AddressValidateRequest",
                new XAttribute("USERID", "400INFIN1745"),
                new XElement("Revision", "1"),
                new XElement("Address",
                new XAttribute("ID", "0"),
                new XElement("Address1", usertAddress.Text),
                new XElement("Address2", userUnitNumber.Text),
                new XElement("City", userCity.Text),
                new XElement("State", userState.Text),
                new XElement("Zip5", userZipcode.Text),
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
                    if (GetXMLElement(element, "DPVConfirmation").Equals("Y") && GetXMLElement(element, "Zip5").Equals(userZipcode.Text) && GetXMLElement(element, "City").Equals(userCity.Text.ToUpper())) // Best case
                    {
                        // Get longitude and latitide because we can make a deliver here. Move on to next page.
                        // Console.WriteLine("The address you entered is valid and deliverable by USPS. We are going to get its latitude & longitude");
                        //GetAddressLatitudeLongitude();
                        Geocoder geoCoder = new Geocoder();

                        IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(usertAddress.Text + "," + userCity.Text + "," + userState.Text);
                        Position position = approximateLocations.FirstOrDefault();

                        latitude = $"{position.Latitude}";
                        longtitude = $"{position.Longitude}";
                        map.MapType = MapType.Satellite;
                        var mapSpan = new MapSpan(position, 0.000001, 0.000001);
                        map.MoveToRegion(mapSpan);

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
                await DisplayAlert("We couldn't find your address", "Please check for errors.", "Ok");
                return;
            }

            await Application.Current.SavePropertiesAsync();
            await tagUser(userEmailAddress.Text, userZipcode.Text);
            Application.Current.MainPage = new NewUI.StartPage();
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
