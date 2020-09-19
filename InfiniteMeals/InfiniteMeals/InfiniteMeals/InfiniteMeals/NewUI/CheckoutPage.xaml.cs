using System;
using System.Collections.Generic;

using Xamarin.Forms;
using static InfiniteMeals.businessItems;

namespace InfiniteMeals.NewUI
{
    public partial class CheckoutPage : ContentPage
    {
        public CheckoutPage(IDictionary<string, ItemPurchased> order)
        {
            InitializeComponent();

            // ZACH THE ORDER INPUT TO THIS FUNCTION CONTAINS THE ORDER COMING
            // BUSINESSITEMS. YOU WOULD HAVE TO ASSIGN THE ORDER VARIABLE TO A
            // GLOBAL ON FOR YOU TO USE IT OUT SIDE THIS SCOPE.
            // TO GET THE REST OF THE INFORMATION ABOUT THE CUSTOMER SUCH AS
            // CUSTOMER'S ID, CUSTOMER'S FIRST NAME, LAST NAME, EMAIL ADDRESS,
            // AND SO FOR YOU CAN GET THIS DATA USING THE FOLLOWING LINE OF CODE
            // AND ITS KEY

                // Application.Current.Properities["userFirstName"] WHERE "userFirstName"
                // IS A KEY.

            // HERE IS THE SET OF KEYS YOU COULD USE FOR THIS PAGE

                // 1. "customer_uid"
                // 2. "userFirstName"
                // 3. "userLastName"
                // 4. "userEmailAddress"
                // 5. "userAddress"
                // 6. "userAddressUnit"
                // 7. "userCity"
                // 8. "userState"
                // 9. "userZipCode"
                // 10. "latitude"
                // 11. "longitude"
                // 12. "userDeliveryInstructions"

            // LET ME KNOW IF YOU HAVE QUESTIONS

        }
        public void onTap(object sender, EventArgs e)
        {

        }

        void DeliveryDaysClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new NewUI.StartPage();
        }

        void OrdersClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new OrdersPage();
        }

        void InfoClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new InfoPage();
        }

        void ProfileClick(System.Object sender, System.EventArgs e)
        {
            Application.Current.MainPage = new profileUser();
        }

        void CardClick(System.Object sender, System.EventArgs e)
        {
            // AGAIN NO ACTION NEEDED
        }
    }
}
