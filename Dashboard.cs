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
        DateTime currentDate, displayDate;
        BottomNavigationView bottomNavigationView;
        Dictionary<string, string> studInfo;
        LinearLayout taskContainer;

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

            taskContainer = FindViewById<LinearLayout>(Resource.Id.taskContainer);

            studInfo = userGetInfo.GetUserDetails(username);

            greetingDisplay.Text = $"Hello, {studInfo["studName"]}";


            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            prevDateBtn = FindViewById<Button>(Resource.Id.previousDateBtn);
            nextDateBtn = FindViewById<Button>(Resource.Id.nextDateBtn);

            prevDateBtn.Click += PreviousDateBtn_Click;
            nextDateBtn.Click += NextDateBtn_Click;

            // Initialize the current date
            currentDate = DateTime.Now;
            displayDate = DateTime.Now;
            SetCurrentDate();


            BottomNavigationViewLogic bottomNav = new BottomNavigationViewLogic(this, bottomNavigationView, username, "Dashboard");            
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_tasks;
            bottomNavigationView.NavigationItemSelected += bottomNav.BottomNavigationView_NavigationItemSelected;
            bottomNav.SetInitialSelectedItem("Dashboard");


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

            LoadTasks(username, currentDate);
        }

        private async void LoadTasks(string username, DateTime date)
        {
            
            var taskHandler = new UserTask(this, taskContainer);
            await taskHandler.GetTasksPerDate(username, date.ToString("dd/MM/yyyy"));
            
        }

        private void SetCurrentDate()
        {
            DateTime todayDate = DateTime.Now;

            string today = todayDate.ToString("MMMM dd, yyyy");

            string formattedDate = displayDate.ToString("MMMM dd, yyyy");

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
            displayDate = displayDate.AddDays(-1); // Go to the previous day
            currentDate = currentDate.AddDays(-1);
            SetCurrentDate();
            LoadTasks(username, currentDate);
        }

        private void NextDateBtn_Click(object sender, EventArgs e)
        {
            displayDate = displayDate.AddDays(1); // Go to the next day
            currentDate = currentDate.AddDays(1);
            SetCurrentDate();
            LoadTasks(username, currentDate);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
