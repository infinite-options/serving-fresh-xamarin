using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
namespace InfiniteMeals.Checkout
{
    public class Coupon
    {
        public string coupon_uid { get; set; }
        public string coupon_id { get; set; }
        public string valid { get; set; }
        public double discount_percent { get; set; }
        public double discount_amount { get; set; }
        public double discount_shipping { get; set; }
        public string expire_date { get; set; }
        public int limits { get; set; }
        public string notes { get; set; }
        public int num_used { get; set; }
        public string recurring { get; set; }
        public string email_id { get; set; }
        public string cup_business_uid { get; set; }


        // Checks if the user associated with email needs to be awarded a Weekly Order Coupon
        // Posts coupon if awarded
        // Returns a task<TResult> indicating whether Post occured successfully (false not awarded/post failed)
        public static async Task<bool> WeeklyOrderCoupon(string userEmail)
        {
            string historyEndpoint = "https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/history/";
            string userEndpoint = String.Concat(historyEndpoint, userEmail);
            // Open Http Connection
            var client = new HttpClient();
            // Get Data from endpoint
            var RDSresponse = await client.GetAsync(userEndpoint);
            var message = await RDSresponse.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(RDSresponse.IsSuccessStatusCode);
            JObject history = JObject.Parse(message);               // Parse Object to get as JObject
            JArray result = history["result"].Value<JArray>();      // Get useful info from Object
            System.Diagnostics.Debug.WriteLine(result);
            List<DateTime> orderDates = new List<DateTime>();
            if (result.Count >= 5)                                  // five orders exists
            {
                foreach (JObject item in result.Children())         // put last 5 order dates into list
                {
                    string sDate = item["purchase_date"].Value<string>();
                    DateTime date = DateTime.Parse(sDate);
                    orderDates.Add(date);
                }
                System.Diagnostics.Debug.WriteLine(String.Join("\n", orderDates));
                for (int i = 0; i < orderDates.Count - 2; i++)                  // look at pairs 0-1, 1-2, 2-3, 3-4
                {
                    TimeSpan interval = orderDates[i + 1] - orderDates[i];      // get interval between orders
                    if (interval.Days > 7)                                       // any interval > 7 days
                    {
                        return false;
                    }
                }
                // If method gets here Coupon should be awarded
                // Set up coupon
                Coupon weeklyOrderCoupon = new Coupon();
                DateTime current = DateTime.Now;
                current.AddMonths(3);
                string expire = current.ToString();

                weeklyOrderCoupon.coupon_uid = "";
                weeklyOrderCoupon.coupon_id = "weeklyOrderCoupon";
                weeklyOrderCoupon.cup_business_uid = "";
                weeklyOrderCoupon.discount_amount = 0;
                weeklyOrderCoupon.discount_percent = 15;
                weeklyOrderCoupon.discount_shipping = 0;
                weeklyOrderCoupon.email_id = userEmail;
                weeklyOrderCoupon.expire_date = expire;
                weeklyOrderCoupon.limits = 1;
                weeklyOrderCoupon.notes = "";
                weeklyOrderCoupon.num_used = 0;
                weeklyOrderCoupon.recurring = "F";

                var formattedCoupon = new StringContent(JsonConvert.SerializeObject(weeklyOrderCoupon), Encoding.UTF8, "application/json");
                var httpResponse = await client.PostAsync(userEndpoint, formattedCoupon);
                Console.WriteLine("This is the order's response " + httpResponse.Content.ReadAsStringAsync());

                return httpResponse.IsSuccessStatusCode;
            }
            return false;
        }
    }
    public class Result
    {
        public string message { get; set; }
        public int code { get; set; }
        public IList<Coupon> result { get; set; }
        public string sql { get; set; }
    }

    public class ServingFreshCoupon
    {
        public string message { get; set; }
        public Result result { get; set; }
    }
    }
