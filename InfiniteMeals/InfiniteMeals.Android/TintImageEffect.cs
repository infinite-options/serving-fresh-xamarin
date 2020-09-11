using System.Linq;
using Android.Graphics;
using Android.Widget;
using Java.Lang;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

using CoreTintImageEffect = InfiniteMeals.TintImageEffect;

[assembly: ResolutionGroupName("MyCompany")]
[assembly: ExportEffect(typeof(InfiniteMeals.Droid.TintImageEffect), nameof(InfiniteMeals.Droid.TintImageEffect))]

namespace InfiniteMeals.Droid
{
    public class TintImageEffect : PlatformEffect
    {
        protected override void OnAttached()
        {
            try
            {
                var effect = (CoreTintImageEffect)Element.Effects.FirstOrDefault(e => e is CoreTintImageEffect);

                if (effect == null || !(Control is ImageView image))
                    return;

                var filter = new PorterDuffColorFilter(effect.TintColor.ToAndroid(), PorterDuff.Mode.SrcIn);
                image.SetColorFilter(filter);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    $"An error occurred when setting the {typeof(TintImageEffect)} effect: {ex.Message}\n{ex.StackTrace}");
            }
        }

        protected override void OnDetached() { }
    }
}