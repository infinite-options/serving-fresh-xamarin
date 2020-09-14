using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using InfiniteMeals.Models;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace InfiniteMeals.NewUI
{
    public partial class MainPage : Xamarin.Forms.TabbedPage
    {
        public MainPage()
        {
            
            SelectedTabColor = Constants.SecondaryColor;
            UnselectedTabColor = Color.Gray;
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            Page days = new NavigationPage(new StartPage());
            days.Title = "Days";
            days.IconImageSource = "CalendarIcon";

            Page orders = new NavigationPage(new StartPage());
            orders.Title = "Orders";
            orders.IconImageSource = "RefundIcon";
            Page info = new NavigationPage(new StartPage());
            info.Title = "Info";
            info.IconImageSource = "InfoIcon";
            Page profile = new NavigationPage(new profileUser());
            profile.Title = "Profile";
            profile.IconImageSource = "UserIcon";
            
            Children.Add(days);
            Children.Add(orders);
            Children.Add(info);
            Children.Add(profile);
        }
    }
}
