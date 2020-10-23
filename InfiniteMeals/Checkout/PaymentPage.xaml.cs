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
    public class StripeResponse
    {
        public string client_secret { get; set; }
        public string id { get; set; }
        public int code { get; set; }
    }

    public partial class PaymentPage : ContentPage
    {
        private PaymentIntentPaymentMethodDataOptions paymentMethodDataOption;

        public PaymentPage()
        {
            InitializeComponent();
        }

        async void placeOrder(object sender, System.EventArgs e)
        {
            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/Stripe_Intent");

            request.Method = HttpMethod.Post;
            var client = new HttpClient();

            // 1. SEND PARVA THE AMOUNT
            // 2. GET A RESPONSE WITH CLIENT SECRET & ID
            // 3. USE THIS CLIENT SECRET AND ID IN SERVICE
            // 4. RECEIVE CONFIMATION FROM STRIPE


            MultipartFormDataContent requestContent = new MultipartFormDataContent();
            StringContent amountContent = new StringContent("10", Encoding.UTF8);
            requestContent.Add(amountContent, "amount");
            request.Content = requestContent;

            HttpResponseMessage response = await client.SendAsync(request);


            Console.WriteLine("Response From Parva: " + response.Content.ReadAsStringAsync());
            // string data = await response.Content.ReadAsStringAsync();
           


            // NEW KEY: pk_test_6RSoSd9tJgB2fN2hGkEDHCXp00MQdrK3Tw
            // SECRET KEY: sk_test_fe99fW2owhFEGTACgW3qaykd006gHUwj1j
            // OLD KEY: "pk_test_j5YImiDFgybfafp8HuEkn6Ou00JtFKI0s9"

            StripeConfiguration.ApiKey = "pk_test_6RSoSd9tJgB2fN2hGkEDHCXp00MQdrK3Tw";

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                var responseString = await content.ReadAsStringAsync();
                var messageObj = JsonConvert.DeserializeObject<StripeResponse>(responseString);

                Console.WriteLine("Client: " + messageObj.client_secret);
                Console.WriteLine("ID: " + messageObj.id);

                var client_secret = messageObj.client_secret;

                var service = new PaymentIntentService();
                var getOptions = new PaymentIntentGetOptions
                {
                    ClientSecret = client_secret
                };

                PaymentIntent paymentIntent = service.Get(messageObj.id, getOptions);
                paymentIntent.Description = "Serving Fresh Order";
                //var creditCardOptions = new CreditCardOptions
                //{
                //    Number = "4000 0000 0000 9995",
                //    Cvc = "333",
                //    ExpMonth = 02,
                //    ExpYear = 24,

                //};

                var paymentMethodCardCreateOption = new PaymentMethodCardCreateOptions
                {
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

                //var paymentMethodDataOption = new PaymentIntentPaymentMethodDataOptions
                //{
                //    Card = paymentMethodCardCreateOption,
                //    Type = "card",

                //};
                List<String> methodTypes = new List<String>();
                methodTypes.Add("card");
                var confirmOptions = new PaymentIntentConfirmOptions();
                //{
                //    //PaymentMethod = "pm_card_visa",
                //    PaymentMethodData = paymentMethodDataOption,

                //    ClientSecret = client_secret,
                //};
                confirmOptions.PaymentMethod = "pm_card_visa";
                confirmOptions.ClientSecret = messageObj.client_secret;
                try
                {
                    var status = service.Confirm(messageObj.id, confirmOptions);
                    Console.WriteLine("This is the status: " + status);
                    if (status.Status == "succeeded")
                    {
                        await DisplayAlert("Request Sent!", "Our customer service will reach out to you in 3-5 bussiness days", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Failed", "Please try another card", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Catch", ex.Message, "OK");
                }
            }
        }
    }
}
