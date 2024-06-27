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
using System;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ViewAccount : AppCompatActivity
    {

        BottomNavigationView bottomNavigationView;
        TextView title, studName, studUsername, studID, studCourse, studSchool;
        Button editBtn, logOutBtn;
        string username, sID, sName, sSchool, sCourse, sIdentity;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.account_layout);

            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);
            studName = FindViewById<TextView>(Resource.Id.studNameTxt);
            studUsername = FindViewById<TextView>(Resource.Id.studUsernameTxt);
            studID = FindViewById<TextView>(Resource.Id.studIDTxt);
            studCourse = FindViewById<TextView>(Resource.Id.studCourseTxt);
            studSchool = FindViewById<TextView>(Resource.Id.studSchoolTxt);
            editBtn = FindViewById<Button>(Resource.Id.editBtn);
            logOutBtn = FindViewById<Button>(Resource.Id.logOutBtn);
            title = FindViewById<TextView>(Resource.Id.title);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(title);
            semiBoldFont.SetFont(studName);
            regularFont.SetFont(studUsername);
            regularFont.SetFont(studID);
            regularFont.SetFont(studCourse);
            regularFont.SetFont(studSchool);
            semiBoldFont.SetFont(editBtn);
            semiBoldFont.SetFont(logOutBtn);

            username = Intent.GetStringExtra("username");
            sID = Intent.GetStringExtra("studID");
            sName = Intent.GetStringExtra("studName");
            sSchool = Intent.GetStringExtra("studSchool");
            sCourse = Intent.GetStringExtra("studCourse");
            sIdentity = Intent.GetStringExtra("studIdentity");


            // Set the selected item to "Account"
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_account;

            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;

            // Set the user's information
            SetStudentInfo();


            ButtonStyler.ApplyRoundedCorners(editBtn);
            ButtonStyler.ApplyRoundedCorners(logOutBtn);

            logOutBtn.Click += logOutBtn_Click;
        }

        
        public void logOutBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Landing));
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
                    // Handle the classes action
                    Toast.MakeText(this, "Classes Layout", ToastLength.Short).Show();
                    nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(ClassesMainView));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(this);

                    break;
                case Resource.Id.navigation_account:

                    break;
            }
        }

        public void SetStudentInfo()
        {
            studName.Text = sName;
            studUsername.Text = "@" + username;
            studID.Text = "Student ID: " + sID;
            studCourse.Text = "Course: "+ sCourse;
            studSchool.Text = "School: "+ sSchool;
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
