using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Google.Android.Material.BottomNavigation;
using Google.Android.Material.FloatingActionButton;
using System;
using System.Collections.Generic;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Dashboard : AppCompatActivity
    {
        TextView dateDisplay, greetingDisplay, desc;
        string username, studID, studSchool, studCourse, studIdentity;
        Button prevDateBtn, nextDateBtn;
        DateTime currentDate;
        BottomNavigationView bottomNavigationView;
        Dictionary<string, string> studInfo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.dashboard_layout);

            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);
            greetingDisplay = FindViewById<TextView>(Resource.Id.greetingTxt);

            desc = FindViewById<TextView>(Resource.Id.desc);

            username = Intent.GetStringExtra("username");

            UserInfoLogic userGetInfo = new UserInfoLogic(this);


            studInfo = userGetInfo.GetUserDetails(username);

            greetingDisplay.Text = $"Hello, {studInfo["studName"]}";


            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            prevDateBtn = FindViewById<Button>(Resource.Id.previousDateBtn);
            nextDateBtn = FindViewById<Button>(Resource.Id.nextDateBtn);

            prevDateBtn.Click += PreviousDateBtn_Click;
            nextDateBtn.Click += NextDateBtn_Click;

            // Initialize the current date
            currentDate = DateTime.Now;

            SetCurrentDate();




            bottomNavigationView.SelectedItemId = Resource.Id.navigation_tasks;
            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(greetingDisplay);
            boldFont.SetFont(dateDisplay);
            regularFont.SetFont(desc);
            semiBoldFont.SetFont(prevDateBtn);
            semiBoldFont.SetFont(nextDateBtn);


            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += (sender, args) =>
            {
                // Handle the click event, e.g., open a new task creation screen
                NextActivityHandler nextActivity = new NextActivityHandler(this, "", typeof(AddNewTask));
                nextActivity.PassDataToNextActivity("username", username);
                //nextActivity.PassDataToNextActivity("classCode", classCode);
                nextActivity.PassDataToNextActivity("layout", "dashboard");
                nextActivity.NavigateToNextActivity(this);
            };

        }

        private void SetCurrentDate()
        {
            DateTime todayDate = DateTime.Now;

            string today = todayDate.ToString("MMMM dd, yyyy");

            string formattedDate = currentDate.ToString("MMMM dd, yyyy");

            if (formattedDate != today)
            {
                desc.Text = "";
            }

            else
            {
                desc.Text = "Here are your tasks for today.";
            }

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

                    break;
                case Resource.Id.navigation_classes:
                    // Handle the classes action
                    Toast.MakeText(this, "Classes Layout", ToastLength.Short).Show();
                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(ClassesMainView));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(this);

                    break;
                case Resource.Id.navigation_account:
                    // Handle the account action
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
