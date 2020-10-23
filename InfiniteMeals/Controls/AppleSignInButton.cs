using System;
using Xamarin.Forms;

namespace InfiniteMeals.Controls
{
    public class AppleSignInButton : ImageButton
    {
        public AppleSignInButton()
        {
            Clicked += AppleSignInButton_Clicked;
            Source = "appleIcon.png";
            //HeightRequest = 100;
            //wi = 100;

            //switch (ButtonStyle)
            //{
            //    case AppleSignInButtonStyle.Black:
            //        BackgroundColor = Color.Black;
            //        TextColor = Color.White;
            //        BorderColor = Color.Black;
            //        break;
            //    case AppleSignInButtonStyle.White:
            //        BackgroundColor = Color.White;
            //        TextColor = Color.Black;
            //        BorderColor = Color.White;
            //        break;
            //    case AppleSignInButtonStyle.WhiteOutline:
            //        BackgroundColor = Color.White;
            //        TextColor = Color.Black;
            //        BorderColor = Color.Black;
            //        break;
            //}

        }

        public AppleSignInButtonStyle ButtonStyle { get; set; } = AppleSignInButtonStyle.Black;

        private void AppleSignInButton_Clicked(object sender, EventArgs e)
        {
            SignIn?.Invoke(sender, e);
            Command?.Execute(CommandParameter);
        }

        public event EventHandler SignIn;

        public void InvokeSignInEvent(object sender, EventArgs e)
            => SignIn?.Invoke(sender, e);

        public void Dispose()
            => Clicked -= AppleSignInButton_Clicked;
    }

    public enum AppleSignInButtonStyle
    {
        Black,
        White,
        WhiteOutline
    }
}
