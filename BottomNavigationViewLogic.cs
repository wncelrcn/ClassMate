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
        Context context;
        BottomNavigationView nav;
        string pageLayout;
        string username;

        public BottomNavigationViewLogic(Context context, BottomNavigationView nav, string username, string pageLayout)
        {
            this.context = context;
            this.nav = nav;
            this.username = username;
            this.pageLayout = pageLayout;

            
        }

        public void SetInitialSelectedItem(string page)
        {
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

        public void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            NextActivityHandler nextActivityHandler;
            switch (e.Item.ItemId)
            {
                case Resource.Id.navigation_tasks:
                    // Handle the tasks action
                    if (pageLayout == "Dashboard")
                    {
                        return;
                    }

                    Toast.MakeText(context, "Tasks Layout", ToastLength.Short).Show();

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

                    Toast.MakeText(context, "Classes Layout", ToastLength.Short).Show();
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

                    Toast.MakeText(context, "Account Layout", ToastLength.Short).Show();

                    nextActivityHandler = new NextActivityHandler(context, "Next...", typeof(ViewAccount));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(context);
                    break;
            }
        }
    }
}
