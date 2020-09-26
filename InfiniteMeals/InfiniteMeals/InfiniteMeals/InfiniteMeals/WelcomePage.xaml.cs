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
using Xamarin.Auth;
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

        public class User
        {
            public string customer_uid { get; set; }
            public string customer_created_at { get; set; }
            public string customer_first_name { get; set; }
            public string customer_last_name { get; set; }
            public string customer_phone_num { get; set; }
            public string customer_email { get; set; }
            public string customer_address { get; set; }
            public string customer_unit { get; set; }
            public string customer_city { get; set; }
            public string customer_state { get; set; }
            public string customer_zip { get; set; }
            public string customer_lat { get; set; }
            public string customer_long { get; set; }
            public object notification_approval { get; set; }
            public object notification_device_id { get; set; }
            public object customer_rep { get; set; }
            public object SMS_freq_preference { get; set; }
            public object SMS_last_notification { get; set; }
            public string password_salt { get; set; }
            public string password_hashed { get; set; }
            public string password_algorithm { get; set; }
            public string referral_source { get; set; }
            public string role { get; set; }
            public object customer_updated_at { get; set; }
            public object email_verified { get; set; }
            public string user_social_media { get; set; }
            public string user_access_token { get; set; }
            public string user_refresh_token { get; set; }
        }

        public class UserAcount
        {
            public string message { get; set; }
            public IList<User> result { get; set; }
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
                builder.Query = "email=" + userEmail.ToLower();

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
                    var user = JsonConvert.DeserializeObject<UserAcount>(message);
                    isUserLoggedIn = httpResponse.IsSuccessStatusCode;

                    Application.Current.Properties["customer_uid"] = user.result[0].customer_uid;
                    Application.Current.Properties["userFirstName"] = user.result[0].customer_first_name;
                    Application.Current.Properties["userLastName"] = user.result[0].customer_last_name;
                    Application.Current.Properties["userEmailAddress"] = user.result[0].customer_email;
                    Application.Current.Properties["userAddress"] = user.result[0].customer_address;
                    Application.Current.Properties["userAddressUnit"] = user.result[0].customer_unit;
                    Application.Current.Properties["userCity"] = user.result[0].customer_city;
                    Application.Current.Properties["userState"] = user.result[0].customer_state;
                    Application.Current.Properties["userZipCode"] = user.result[0].customer_zip;
                    Application.Current.Properties["latitude"] = user.result[0].customer_lat;
                    Application.Current.Properties["longitude"] = user.result[0].customer_long;
                    Application.Current.Properties["userDeliveryInstructions"] = "";
                    Application.Current.Properties["userPhoneNumber"] = user.result[0].customer_phone_num;

                    Console.WriteLine("This is your response content = " + message);
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

        // THIS FUNCTION WILL START THE FLOW TO LOG IN WITH USER'S FACEBOOK
        // LOG IN CREDENTIALS
        async void FacebookLogInClick(System.Object sender, System.EventArgs e)
        {
            await DisplayAlert("You clicked Facebook BUTTON", "", "OK");
        }

        // THIS FUNCTION WILL START THE FLOW TO LOG IN WITH USER'S GOOGLE
        // LOG IN CREDENTIALS
        private void GoogleLogInClick(System.Object sender, System.EventArgs e)
        {
            string clientId = null;
            string redirectUri = null;
            string scope = "https://www.googleapis.com/auth/userinfo.profile https://www.googleapis.com/auth/userinfo.email";
            string AuthorizeUrl = "https://accounts.google.com/o/oauth2/v2/auth";
            string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    clientId = "97916302968-f22boafqno1dicq4a0eolpr6qj8hkvbm.apps.googleusercontent.com";
                    redirectUri = "com.googleusercontent.apps.97916302968-f22boafqno1dicq4a0eolpr6qj8hkvbm:/oauth2redirect";
                    break;

                case Device.Android:
                    clientId = "97916302968-6nlu2otc3icdefg28qpbqbk1fam2hj8d.apps.googleusercontent.com";
                    redirectUri = "com.infiniteoptions.socialloginsxamarin:/oauth2redirect";
                    break;
            }

            var authenticator = new OAuth2Authenticator(
                clientId,
                "",
                scope,
                new Uri(AuthorizeUrl),
                new Uri(redirectUri),
                new Uri(AccessTokenUrl),
                null,
                true);

            authenticator.Completed += Authenticator_Completed;
            authenticator.Error += Authenticator_Error;

            AuthenticationState.Authenticator = authenticator;

            var presenter = new Xamarin.Auth.Presenters.OAuthLoginPresenter();
            presenter.Login(authenticator);

        }


    private void Authenticator_Error(object sender, AuthenticatorErrorEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }

            DisplayAlert("Authentication error: ", e.Message, "OK");
        }

        private async void Authenticator_Completed(object sender, AuthenticatorCompletedEventArgs e)
        {
            var authenticator = sender as OAuth2Authenticator;
            if (authenticator != null)
            {
                Console.WriteLine("The authenticator is null");
                authenticator.Completed -= Authenticator_Completed;
                authenticator.Error -= Authenticator_Error;
            }
            Console.WriteLine("This information is comming from the event" + e.IsAuthenticated);
            Console.WriteLine("This information is comming from the event" + e.Account);
            if (e.IsAuthenticated)
            {

                Application.Current.Properties["customer_uid"] = "100-000097";
                Application.Current.Properties["userFirstName"] = "Google User First Name";
                Application.Current.Properties["userLastName"] = "Google User Last Name";
                Application.Current.Properties["userEmailAddress"] = "Empty";
                Application.Current.Properties["userAddress"] = "Empty";
                Application.Current.Properties["userAddressUnit"] = "Empty";
                Application.Current.Properties["userCity"] = "Empty";
                Application.Current.Properties["userState"] = "Empty";
                Application.Current.Properties["userZipCode"] = "Empty";
                Application.Current.Properties["latitude"] = "0.0";
                Application.Current.Properties["longitude"] = "0.0";
                Application.Current.Properties["userDeliveryInstructions"] = "Empty";
                Application.Current.Properties["userPhoneNumber"] = "Empty";

                Application.Current.MainPage = new NewUI.StartPage();

                // If the user is authenticated, request their basic user data from Google
                string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";
                var request = new OAuth2Request("GET", new Uri(UserInfoUrl), null, e.Account);
                var response = await request.GetResponseAsync();
                string userJsonString = await response.GetResponseTextAsync();
                var str = JObject.Parse(userJsonString);
                Console.WriteLine("THIS IS THE RESPONSE FROM GET REQUEST" + str);
                Console.WriteLine(e.Account.Properties["access_token"]);
                Console.WriteLine(e.Account.Properties["refresh_token"]);
                //if (response != null)
                //{
                //	// Deserialize the data and store it in the account store
                //	// The users email address will be used to identify data in SimpleDB
                //	string userJsonString = await response.GetResponseTextAsync();
                //	userJson = JObject.Parse(userJsonString);
                //}

            }
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
