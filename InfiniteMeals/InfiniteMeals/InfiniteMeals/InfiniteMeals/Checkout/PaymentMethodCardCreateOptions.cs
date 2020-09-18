// A page for testing the Stripe API, not actually used in the app.
namespace InfiniteMeals.Checkout
{
    internal class PaymentMethodCardCreateOptions
    {
        public string Card { get; set; }
        public string Number { get; set; }
        public string Cvc { get; set; }
        public long ExpMonth { get; set; }
        public long ExpYear { get; set; }
    }
}