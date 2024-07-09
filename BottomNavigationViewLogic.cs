using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Google.Android.Material.BottomNavigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IT123P_FinalMP
{
    internal class BottomNavigationViewLogic
    {
        // Variables
        Context context;
        BottomNavigationView nav;
        string pageLayout;
        string username;

        // Constructor
        public BottomNavigationViewLogic(Context context, BottomNavigationView nav, string username, string pageLayout)
        {
            this.context = context;
            this.nav = nav;
            this.username = username;
            this.pageLayout = pageLayout;

            
        }

        // Set the initial selected item in the BottomNavigationView
        public void SetInitialSelectedItem(string page)
        {
            // Set the selected item in the BottomNavigationView
            switch (page)
            {
                case "Dashboard":
                    nav.SelectedItemId = Resource.Id.navigation_tasks;
                    break;
                case "ClassesMainView":
                    nav.SelectedItemId = Resource.Id.navigation_classes;
                    break;
                case "ViewAccount":
                    nav.SelectedItemId = Resource.Id.navigation_account;
                    break;
            }
        }
        // Handle the BottomNavigationView's NavigationItemSelected event
        public void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            // Handle the BottomNavigationView's NavigationItemSelected event
            NextActivityHandler nextActivityHandler;
            switch (e.Item.ItemId)
            {
                
                case Resource.Id.navigation_tasks:
                    // Handle the tasks action
                    if (pageLayout == "Dashboard")
                    {
                        return;
                    }
                    // Navigate to the Dashboard activity
                    nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(Dashboard));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(context);
                    break;
                case Resource.Id.navigation_classes:
                    // Handle the classes action
                    if (pageLayout == "ClassesMainView")
                    {
                        return;
                    }
                    // Navigate to the ClassesMainView activity
                    nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ClassesMainView));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(context);

                    break;
                case Resource.Id.navigation_account:
                    // Handle the account action
                    if (pageLayout == "ViewAccount")
                    {
                        return;
                    }
                    // Navigate to the ViewAccount activity
                    nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ViewAccount));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(context);
                    break;
            }
        }
    }
}
