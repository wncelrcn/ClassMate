using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Text.Method;
using Android.Text.Style;
using Android.Text;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.Content;
using Google.Android.Material.BottomNavigation;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ViewAccount : AppCompatActivity
    {

        BottomNavigationView bottomNavigationView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.account_layout);

            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);


            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            // Set the selected item to "Account"
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_account;

            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;
        }


        private void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.navigation_tasks:
                    // Handle the tasks action
                    Toast.MakeText(this, "Tasks Layout", ToastLength.Short).Show();

                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Dashboard));
                    nextActivityHandler.NavigateToNextActivity(this);
                    break;
                case Resource.Id.navigation_classes:
                    // Handle the classes action
                    Toast.MakeText(this, "Classes Layout", ToastLength.Short).Show();
                    break;
                case Resource.Id.navigation_account:
                    
                    break;
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
