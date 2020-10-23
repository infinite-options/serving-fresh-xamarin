using System;
using InfiniteMeals;
using InfiniteMeals.iOS.Renderers;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace InfiniteMeals.iOS.Renderers
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                //Below line is useful to give border color 
                Control.TintColor = UIColor.Blue;
                Control.Layer.CornerRadius = 0;
                Control.TextColor = UIColor.Black;
            }
        }
    }
}
