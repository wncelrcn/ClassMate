using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.Content;
using Google.Android.Material.BottomNavigation;
using System;
using System.Threading.Tasks;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClassesMainView : AppCompatActivity
    {

        BottomNavigationView bottomNavigationView;
        Button btnAddClass;
        LinearLayout classesLayoutContainer;
        string username;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.classes_main_layout);

            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);

            username = Intent.GetStringExtra("username");

            classesLayoutContainer = FindViewById<LinearLayout>(Resource.Id.classContainer);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");


            btnAddClass = FindViewById<Button>(Resource.Id.addAClassBtn);
            btnAddClass.Click += AddClass;

            bottomNavigationView.SelectedItemId = Resource.Id.navigation_classes;

            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;

            LoadClass();
        }

        public async void LoadClass()
        {
            UserClass userClass = new UserClass(this, classesLayoutContainer);
            await userClass.GetCurrStudClasses(username);
        }
      


        public void AddClass(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesAddView));
            nextActivityHandler.PassDataToNextActivity("username", username);

            nextActivityHandler.NavigateToNextActivity(this);
        }





        private void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.navigation_tasks:
                    // Handle the tasks action
                    Toast.MakeText(this, "Tasks Layout", ToastLength.Short).Show();

                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Dashboard));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(this);
                    break;
                case Resource.Id.navigation_classes:
                    break;
                case Resource.Id.navigation_account:
                    Toast.MakeText(this, "Account Layout", ToastLength.Short).Show();

                    nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(ViewAccount));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(this);
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