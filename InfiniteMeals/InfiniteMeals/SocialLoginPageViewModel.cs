using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Newtonsoft.Json;
using Plugin.GoogleClient;
using Plugin.GoogleClient.Shared;
using Xamarin.Forms;

namespace InfiniteMeals
{
    public class SocialLoginPageViewModel
    {
        public ICommand OnLoginCommand { get; set; }
        IGoogleClientManager _googleService = CrossGoogleClient.Current;
        public ObservableCollection<AuthNetwork> AuthenticationNetworks { get; set; } = new ObservableCollection<AuthNetwork>()
        {
              new AuthNetwork()
            {
                Name = "Google",
                Icon = "ic_google",
                Foreground = "#000000",
                Background ="#F8F8F8"
            }
        };
        public SocialLoginPageViewModel()
        {

            OnLoginCommand = new Command<AuthNetwork>(async (data) => await LoginAsync(data));
        }
        async Task LoginAsync(AuthNetwork authNetwork)
        {
            switch (authNetwork.Name)
            {
                //case "Facebook":
                //    await LoginFacebookAsync(authNetwork);
                //    break;
                //case "Instagram":
                //    await LoginInstagramAsync(authNetwork);
                //    break;
                case "Google":
                    await LoginGoogleAsync(authNetwork);
                    break;
            }
        }
        async Task LoginGoogleAsync(AuthNetwork authNetwork)
        {
            try
            {
                //if (!string.IsNullOrEmpty(_googleService.ActiveToken))
                //{
                //    //Always require user authentication
                //    _googleService.Logout();
                //}
                Console.WriteLine("in google login");
                if (!string.IsNullOrEmpty(_googleService.AccessToken))
                {
                    //Always require user authentication
                    _googleService.Logout();
                }
                EventHandler<GoogleClientResultEventArgs<GoogleUser>> userLoginDelegate = null;
                Console.WriteLine("before userLogin");
                userLoginDelegate = async (object sender, GoogleClientResultEventArgs<GoogleUser> e) =>
                {
                    Console.WriteLine("e.Status");
                    switch (e.Status)
                    {
                        case GoogleActionStatus.Completed:
#if DEBUG
                            var googleUserString = JsonConvert.SerializeObject(e.Data);
                            Debug.WriteLine($"Google Logged in succesfully: {googleUserString}");
#endif

                            var socialLoginData = new NetworkAuthData
                            {
                                Id = e.Data.Id,
                                Logo = authNetwork.Icon,
                                Foreground = authNetwork.Foreground,
                                Background = authNetwork.Background,
                                Picture = e.Data.Picture.AbsoluteUri,
                                Name = e.Data.Name,
                            };

                            await App.Current.MainPage.Navigation.PushModalAsync(new HomePage(socialLoginData));
                            break;
                        case GoogleActionStatus.Canceled:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Canceled", "Ok");
                            break;
                        case GoogleActionStatus.Error:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Error", "Ok");
                            break;
                        case GoogleActionStatus.Unauthorized:
                            await App.Current.MainPage.DisplayAlert("Google Auth", "Unauthorized", "Ok");
                            break;
                    }
                    Console.WriteLine("here");
                    _googleService.OnLogin -= userLoginDelegate;
                };

                _googleService.OnLogin += userLoginDelegate;

                await _googleService.LoginAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("HAHA");
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}
