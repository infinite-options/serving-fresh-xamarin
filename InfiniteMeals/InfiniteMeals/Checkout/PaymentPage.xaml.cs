// A page for testing the Stripe API, not actually used in the app.
using System;
using System.Collections.Generic;
using System.Net.Http;
using Xamarin.Forms;
using Stripe;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

namespace InfiniteMeals.Checkout
{
    public class message
    {
        public string client_secret { get; set; }
        public string id { get; set; }
    }

    public partial class PaymentPage : ContentPage
    {
        public PaymentPage()
        {
            InitializeComponent();
        }

        async void placeOrder(object sender, System.EventArgs e)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("http://10.0.2.2:5000/api/v1/payment");

            request.Method = HttpMethod.Get;
            var client = new HttpClient();
            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            StringContent amountContent = new StringContent("9923", Encoding.UTF8);
            requestContent.Add(amountContent, "amount");
            request.Content = requestContent;


            HttpResponseMessage response = await client.SendAsync(request);
            StripeConfiguration.ApiKey = "pk_test_j5YImiDFgybfafp8HuEkn6Ou00JtFKI0s9";

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
                PaymentIntent paymentIntent = service.Get(messageObj.id,getOptions);

                //var creditCardOptions = new CreditCardOptions
                //{
                //    Number = "4000 0000 0000 9995",
                //    Cvc = "333",
                //    ExpMonth = 02,
                //    ExpYear = 24,
                    
                //};

                var paymentMethodCardCreateOption = new PaymentMethodCardCreateOptions {
                    //Insufficiant fund
                    //Number = "4000000000009995",
                    //Payment Succeed
                    //Number = "4242424242424242",
                    //Payment Require Authentication
                    //Number = "4000002500003155",
                    Number = CreditCardNumber.Text,
                    Cvc = CVC.Text,
                    ExpMonth = long.Parse(ExpMonth.Text),
                    ExpYear = long.Parse(ExpYear.Text),
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
                    
                    ClientSecret = client_secret,
                };
                try
                {
                    var status = service.Confirm(messageObj.id, confirmOptions);
                    if (status.Status == "succeeded")
                    {
                        await DisplayAlert("Request Sent!", "Our customer service will reach out to you in 3-5 bussiness days", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Failed", "Please try another card", "OK");
                    }
                }
                catch (Exception)
                {
                    await DisplayAlert("Failed", "Please try another card", "OK");
                }


                //Console.WriteLine(status.Status);
            }
        }
    }
}
