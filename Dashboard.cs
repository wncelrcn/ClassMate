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
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Dashboard : AppCompatActivity
    {

        // widget declarations
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
            
            SetContentView(Resource.Layout.dashboard_layout);

            // widget initialization
            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);
            greetingDisplay = FindViewById<TextView>(Resource.Id.greetingTxt);
            desc = FindViewById<TextView>(Resource.Id.desc);
            taskContainer = FindViewById<LinearLayout>(Resource.Id.taskContainer);
            dateDisplay = FindViewById<TextView>(Resource.Id.dateDisplay);
            prevDateBtn = FindViewById<Button>(Resource.Id.previousDateBtn);
            nextDateBtn = FindViewById<Button>(Resource.Id.nextDateBtn);
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            // fetch username from the intent
            username = Intent.GetStringExtra("username");

            // fetch student info
            UserInfoLogic userGetInfo = new UserInfoLogic(this);
            studInfo = userGetInfo.GetUserDetails(username);
            // set the greeting text
            greetingDisplay.Text = $"Hello, {studInfo["studName"]}";

            // Initialize the current date
            currentDate = DateTime.Now;
            displayDate = DateTime.Now;
            SetCurrentDate();

            // bottom navigation view logic
            BottomNavigationViewLogic bottomNav = new BottomNavigationViewLogic(this, bottomNavigationView, username, "Dashboard");            
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_tasks;
            bottomNavigationView.NavigationItemSelected += bottomNav.BottomNavigationView_NavigationItemSelected;
            bottomNav.SetInitialSelectedItem("Dashboard");

            // font styles
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            
            semiBoldFont.SetFont(greetingDisplay);
            boldFont.SetFont(dateDisplay);
            regularFont.SetFont(desc);
            semiBoldFont.SetFont(prevDateBtn);
            semiBoldFont.SetFont(nextDateBtn);

            // event handlers for the buttons
            prevDateBtn.Click += PreviousDateBtn_Click;
            nextDateBtn.Click += NextDateBtn_Click;
            
            // event handler for the floating action button
            fab.Click += (sender, args) =>
            {
                // Handle the click event, e.g., open a new task creation screen
                NextActivityHandler nextActivity = new NextActivityHandler(this, "", typeof(AddNewTask));
                nextActivity.PassDataToNextActivity("username", username);
                //nextActivity.PassDataToNextActivity("classCode", classCode);
                nextActivity.PassDataToNextActivity("layout", "dashboard");
                nextActivity.NavigateToNextActivity(this);
            };

            // load the tasks
            LoadTasks(username, currentDate);
        }


        // method to load the tasks
        private async void LoadTasks(string username, DateTime date)
        {
            // instantiate the UserTask class
            var taskHandler = new UserTask(this, taskContainer);

            // get the tasks for the current date
            await taskHandler.GetTasksPerDate(username, date.ToString("dd/MM/yyyy"));
            
        }

        // method to set the current date
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

        // event handler for the previous date button
        private void PreviousDateBtn_Click(object sender, EventArgs e)
        {
            displayDate = displayDate.AddDays(-1); // Go to the previous day
            currentDate = currentDate.AddDays(-1); 
            SetCurrentDate();
            LoadTasks(username, currentDate);
        }
        
        // event handler for the next date button
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
