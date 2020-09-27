using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using System.Diagnostics;
using Acr.UserDialogs;
using Xamarin.Essentials;

namespace InfiniteMeals.Refund
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductRefundPage : ContentPage
    {
        Stream photoStream = null;

        public ProductRefundPage()
        {
            InitializeComponent();
            CameraButton.Clicked += CameraButton_Clicked;
            GalleryButton.Clicked += ImageGalleryButton_Clicked;
            PhotoImage.Source = "https://cdn.shopify.com/s/files/1/0533/2089/files/placeholder-images-image_large.png";
        }

        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { SaveToAlbum = true });

                if (photo != null)
                {
                    //Get the public album path
                    var aPpath = photo.AlbumPath;

                    //Get private path
                    var path = photo.Path;
                    photoStream = photo.GetStream();
                    PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                }
            }
            catch
            {
                await DisplayAlert("Permission required", "We'll need permission to access your camara, so that you can take a photo of the damaged product.", "OK");
                return;
            }
        }

        private async void ImageGalleryButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var photo = await Plugin.Media.CrossMedia.Current.PickPhotoAsync();

                if (photo != null)
                {
                    photoStream = photo.GetStream();
                    PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

                }
            }
            catch
            {
                await DisplayAlert("Permission required", "We'll need permission to access your camara roll, so that you can select a photo of the damaged product.", "OK");
                return;
            }
        }

        // SEND REFUNDS REQUEST WAS UPDATED BY CARLOS
        async void sendRefundRequest(object sender, EventArgs e)
        {   
            if(photoStream == null)
            {
                await DisplayAlert("Missing photo", "Please take a photo of your damage product with the button below", "OK");
                return;
            }
            if(email.Text == null)
            {
                await DisplayAlert("Email can't be empty", "Please fill in your email", "OK");
                return;
            }
            if (message.Text == null)
            {
                await DisplayAlert("Message can't be empty", "Please fill in the message", "OK");
                return;
            }
            try
            {

                var userEmail = email.Text;
                var userMessage = message.Text;
                var userPhone = "4158329643";
                var userImage = PhotoImage.Source;

                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                StringContent userEmailContent = new StringContent(userEmail, Encoding.UTF8);
                StringContent userPhoneContent = new StringContent(userPhone, Encoding.UTF8);
                StringContent userMessageContent = new StringContent(userMessage, Encoding.UTF8);

                var ms = new MemoryStream();
                photoStream.CopyTo(ms);
                byte[] TargetImageByte = ms.ToArray();
                ByteArrayContent userImageContent = new ByteArrayContent(TargetImageByte);

                content.Add(userEmailContent, "client_email");
                content.Add(userPhoneContent, "client_phone");
                content.Add(userMessageContent, "client_message");

                // CONTENT, NAME, FILENAME
                content.Add(userImageContent, "product_image", "product_image.png");

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("https://tsx3rnuidi.execute-api.us-west-1.amazonaws.com/dev/api/v2/refundDetailsNEW");
                request.Method = HttpMethod.Post;
                request.Content = content;

                UserDialogs.Instance.ShowLoading("Sending your request...");
                HttpResponseMessage response = await client.SendAsync(request);

                Console.WriteLine("This is the response from request" + response);
                UserDialogs.Instance.HideLoading();
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Request Sent!", "Give us a day or two to respond.", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Fail!", "Sorry! Something went wrong", "OK");
                    await Navigation.PopAsync();
                }
                return;
            }
            catch (Exception exception)
            {
                Debug.WriteLine("Exception Caught: " + exception.ToString());
                return;
            }
        }
    }
}