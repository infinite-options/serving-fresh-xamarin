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
            PhotoImage.Source = "https://cdn.shopify.com/s/files/1/0533/2089/files/placeholder-images-image_large.png";
        }
        private async void CameraButton_Clicked(object sender, EventArgs e)
        {
            var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
            {
                photoStream = photo.GetStream();
                PhotoImage.Source = ImageSource.FromStream(() => { return photo.GetStream(); });
                
            }
        }

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
                var userImage = PhotoImage.Source;
                HttpClient client = new HttpClient();
                MultipartFormDataContent content = new MultipartFormDataContent();
                StringContent userEmailContent = new StringContent(userEmail, Encoding.UTF8);
                StringContent userMessageContent = new StringContent(userMessage, Encoding.UTF8);

                var ms = new MemoryStream();
                photoStream.CopyTo(ms);
                byte[] TargetImageByte = ms.ToArray();
                ByteArrayContent userImageContent = new ByteArrayContent(TargetImageByte);
                content.Add(userEmailContent, "client_email");
                content.Add(userMessageContent, "client_message");
                content.Add(userImageContent, "product_image", "product_image.png");

                var request = new HttpRequestMessage();
                request.RequestUri = new Uri("http://10.0.2.2:5000/api/v1/refund");
                request.Method = HttpMethod.Post;
                request.Content = content;
                HttpResponseMessage response = await client.SendAsync(request);
                if(response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Request Sent!", "Our customer service will reach out to you in 3-5 bussiness days", "OK");
                    Navigation.PopModalAsync();
                }
                else
                {
                    await DisplayAlert("Fail!", "Sorry! Something went wrong", "OK");
                    Navigation.PopModalAsync();
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