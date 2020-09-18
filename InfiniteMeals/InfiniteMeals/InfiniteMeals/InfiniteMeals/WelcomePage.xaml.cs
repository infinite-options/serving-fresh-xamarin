using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xamarin.Forms;

namespace InfiniteMeals
{
    public partial class logInPage : ContentPage
    {
        // GLOBAL VARIABLE AND ENDPOINTS
        public Boolean termsOfServiceChecked = false;
        public Boolean weeklyUpdatesChecked = false;
        public HttpClient client = new HttpClient();
        public const string signUpApi = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/signup";
        const string accountSaltURL = "https://uavi7wugua.execute-api.us-west-1.amazonaws.com/dev/api/v2/accountsalt/"; // api to get account salt; need email at the end of link
        const string loginURL = "https://www.google.com/url?q=https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/AccountSalt/"; // api to log in; need email + hashed password at the end of link
        public bool isUserLoggedIn = false;

        // THE FOLLOWING CLASSES STORE THE PASSWORD_ALGORITHM, PASSWORD_SALT, AND
        // THE USER INFORMATION WHICH IS SEND TO THE DATABASE TO AUTH USER'S
        // CREDENTIALS

        public class UserInfo
        {
            public string email { get; set; }
            public string password { get; set; }
        }

        public class AccountSalt
        {
            public string password_algorithm { get; set; }
            public string password_salt { get; set; }
        }

        public class AccountSaltResponse
        {
            public string message { get; set; }
            public IList<AccountSalt> result { get; set; }
        }

        //======================================================================

        // WELCOME PAGE MAIN FUNCTION
        public logInPage()
        {
            InitializeComponent();
        }

        // INFINITE OPTIONS LOGIN IMPLEMENTATION
        private async void InfiniteOptionsLogInClick(System.Object sender, System.EventArgs e)
        {
            // await DisplayAlert("You clicked LOGIN BUTTON", "", "OK");
            logInButton.IsEnabled = false;
            if (String.IsNullOrEmpty(this.userEmailAddress.Text) || String.IsNullOrEmpty(this.userPassword.Text))
            { // check if all fields are filled out
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                logInButton.IsEnabled = true;
            }
            else
            {
                var accountSalt = await retrieveAccountSalt(this.userEmailAddress.Text); // retrieve user's account salt
                login(this.userEmailAddress.Text, this.userPassword.Text, accountSalt);
            }
        }

        // THIS FUNCTION RETURNS RETRIVES THE ACCOUNT SALT CREDENTIALS FROM
        // DATABASE BY AUTHENTICATING USER'S EMAIL ADDRESS
        private async Task<AccountSalt> retrieveAccountSalt(string userEmail)
        {
            try
            {
                // https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/AccountSalt/?email=annrupp22%40gmail.com

                UriBuilder builder = new UriBuilder("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/AccountSalt");
                builder.Query = "email=quang@gmail.com";

                // Console.WriteLine("builder " + builder);
                // Console.WriteLine("builderq " + builder.Query);

                var result = await client.GetStringAsync(builder.Uri);

                AccountSaltResponse data = new AccountSaltResponse();
                data = JsonConvert.DeserializeObject<AccountSaltResponse>(result);

                AccountSalt userInformation = new AccountSalt
                {
                    password_algorithm = data.result[0].password_algorithm,
                    password_salt = data.result[0].password_salt
                };

                // Console.WriteLine("Line 349: This is the password_algorithm = " + userInformation.password_algorithm);
                // Console.WriteLine("Line 350: This is the password_salt = " + userInformation.password_salt);

                return userInformation;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // THIS FUNCTION IS LOGS IN THE USER IF THEIR ACCOUNT INFORMATION MATCHES
        // WITH DATABASE KEYS
        private async void login(string userEmail, string userPassword, AccountSalt accountSalt)
        {
            const string deviceBrowserType = "Mobile";
            var deviceIpAddress = Dns.GetHostAddresses(Dns.GetHostName()).FirstOrDefault();

            //var deviceIpAddress = "0.0.0.0";
            if (deviceIpAddress != null)
            {
                try
                {
                    SHA512 sHA512 = new SHA512Managed();
                    Console.WriteLine("sha " + sHA512);

                    byte[] data = sHA512.ComputeHash(Encoding.UTF8.GetBytes(userPassword + accountSalt.password_salt)); // take the password and account salt to generate hash
                    Console.WriteLine("data " + data[0]);

                    string hashedPassword = BitConverter.ToString(data).Replace("-", string.Empty).ToLower(); // convert hash to hex

                    UserInfo ui = new UserInfo()
                    {
                        email = userEmail,
                        password = hashedPassword,

                    };

                    var data2 = JsonConvert.SerializeObject(ui);
                    var content = new StringContent(data2, Encoding.UTF8, "application/json");

                    using (var httpClient = new HttpClient())
                    {
                        var request = new HttpRequestMessage();
                        request.Method = HttpMethod.Post;
                        request.Content = content;
                        var httpResponse = await httpClient.PostAsync("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/Login", content);
                        var message = await httpResponse.Content.ReadAsStringAsync();
                        isUserLoggedIn = httpResponse.IsSuccessStatusCode;

                        Console.WriteLine("This is the JSON object = " + httpResponse.IsSuccessStatusCode);
                        Console.WriteLine("This is the value of isUserLoggedIn = " + isUserLoggedIn);

                        if (isUserLoggedIn)
                        {
                            Application.Current.MainPage = new NewUI.StartPage();
                        }
                        else
                        {
                            await DisplayAlert("Log In Message", "It looks like your weren't able to log in. Try one more time!", "OK");
                        }
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine("Exception message: " + e.Message);
                }
            }
        }

        // THIS FUNCTION WILL START THE FLOW TO LOG IN WITH USER'S FACEBOOK
        // LOG IN CREDENTIALS
        async void FacebookLogInClick(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("You clicked Facebook BUTTON", "", "OK");
        }

        // THIS FUNCTION WILL START THE FLOW TO LOG IN WITH USER'S GOOGLE
        // LOG IN CREDENTIALS
        async void GoogleLogInClick(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("You clicked Google BUTTON", "", "OK");
        }

        // THIS FUNCTION WILL START THE FLOW TO LOG IN WITH USER'S APPLE
        // LOG IN CREDENTIALS
        async void AppleLogInClick(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("You clicked Apple BUTTON", "", "OK");
        }

        // THIS FUNCTION WILL SEND YOU TO THE PROCEEDASGUEST PAGE UPON
        // A CLICK EVENT
        void ProceedAsGuestClick(System.Object sender, System.EventArgs e)
        {
            // await DisplayAlert("You clicked ProccedAsGuest BUTTON", "", "OK");
            Application.Current.MainPage = new ProceedAsGuest();
        }

        // THIS FUNCTION WILL SEND YOU TO THE SIGNUP PAGE UPON
        // A CLICK EVENT
        void SignUpClick(System.Object sender, System.EventArgs e)
        {
            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/shell/navigation
            // await DisplayAlert("You clicked SignUp BUTTON", "", "OK");
            Application.Current.MainPage = new signUpPage();
        }
    }
}
