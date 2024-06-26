using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using System;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Dashboard : AppCompatActivity
    {
        TextView dateDisplay;
        string username, password, studID, studName, studSchool, studCourse, studIdentity;
        Button prevDateBtn, nextDateBtn;
        DateTime currentDate;
        BottomNavigationView bottomNavigationView;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.dashboard_layout);

            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);
            
            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");
            studID = Intent.GetStringExtra("studID");
            studName = Intent.GetStringExtra("studName");
            studSchool = Intent.GetStringExtra("studSchool");
            studCourse = Intent.GetStringExtra("studCourse");
            studIdentity = Intent.GetStringExtra("studIdentity");

            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            prevDateBtn = FindViewById<Button>(Resource.Id.previousDateBtn);
            nextDateBtn = FindViewById<Button>(Resource.Id.nextDateBtn);

            prevDateBtn.Click += PreviousDateBtn_Click;
            nextDateBtn.Click += NextDateBtn_Click;

            // Initialize the current date
            currentDate = DateTime.Now;

            SetCurrentDate();

            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;
        }

        private void SetCurrentDate()
        {
            string formattedDate = currentDate.ToString("MMMM dd, yyyy");
            dateDisplay.Text = formattedDate;
        }

        private void PreviousDateBtn_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddDays(-1); // Go to the previous day
            SetCurrentDate();
        }

        private void NextDateBtn_Click(object sender, EventArgs e)
        {
            currentDate = currentDate.AddDays(1); // Go to the next day
            SetCurrentDate();
        }

        private void BottomNavigationView_NavigationItemSelected(object sender, BottomNavigationView.NavigationItemSelectedEventArgs e)
        {
            switch (e.Item.ItemId)
            {
                case Resource.Id.navigation_tasks:
                    // Handle the tasks action
                    Toast.MakeText(this, "Tasks selected", ToastLength.Short).Show();
                    break;
                case Resource.Id.navigation_classes:
                    // Handle the classes action
                    Toast.MakeText(this, "Classes selected", ToastLength.Short).Show();
                    break;
                case Resource.Id.navigation_account:
                    // Handle the account action
                    Toast.MakeText(this, "Account selected", ToastLength.Short).Show();
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
