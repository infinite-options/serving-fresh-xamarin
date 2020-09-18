﻿using System;
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

namespace InfiniteMeals
{
    public partial class ProceedAsGuest : ContentPage
    {
        public ProceedAsGuest()
        {
            InitializeComponent();
        }

        async void ProceedAsGuestClick(object sender, System.EventArgs e)
        {
            if (userAddress.Text != null)
            {
                userAddress.Text = userAddress.Text.Trim();
                Application.Current.Properties["street"] = userAddress.Text;
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
                new XElement("Address1", userAddress.Text),
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
                        // GetAddressLatitudeLongitude();
                        Geocoder geoCoder = new Geocoder();

                        IEnumerable<Position> approximateLocations = await geoCoder.GetPositionsForAddressAsync(userAddress.Text + "," + userCity.Text + "," + userState.Text);
                        Position position = approximateLocations.FirstOrDefault();

                        latitude = $"{position.Latitude}";
                        longtitude = $"{position.Longitude}";
                        break;
                    }
                    else if (GetXMLElement(element, "DPVConfirmation").Equals("D"))
                    {
                        // await DisplayAlert("Alert!", "Address is missing information like 'Apartment number'.", "Ok");
                        // return;
                    }
                    else
                    {
                        // await DisplayAlert("Alert!", "Seems like your address is invalid.", "Ok");
                        // return;
                    }
                }
                else
                {   // USPS sents an error saying address not found in there records. In other words, this address is not valid because it does not exits.
                    // Console.WriteLine("Seems like your address is invalid.");
                    // await DisplayAlert("Alert!", "Error from USPS. The address you entered was not found.", "Ok");
                    // return;
                }
            }
            if (latitude == "0" || longtitude == "0")
            {
                await DisplayAlert("We couldn't find your address", "Please check for errors.", "Ok");
                return;
            }

            await Application.Current.SavePropertiesAsync();
            Application.Current.MainPage = new NewUI.StartPage();
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

        void EnterEmailPasswordToSignUpClick(System.Object sender, System.EventArgs e)
        {
            // Console.WriteLine("You are about to transition to the SignUpPage");
            Application.Current.MainPage = new signUpPage();
        }
    }
}
