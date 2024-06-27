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
using System.Collections.Generic;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ViewAccount : AppCompatActivity
    {

        BottomNavigationView bottomNavigationView;
        TextView title, studName, studUsername, studID, studCourse, studSchool;
        Button editStudInfoBtn, logOutBtn, editAccBtn;
        string username, sID, sName, sSchool, sCourse, sIdentity;
        Dictionary<string, string> studInfo;

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
            editStudInfoBtn = FindViewById<Button>(Resource.Id.editStudInfoBtn);
            editAccBtn = FindViewById<Button>(Resource.Id.editAccBtn);
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
            semiBoldFont.SetFont(editStudInfoBtn);
            semiBoldFont.SetFont(logOutBtn);
            semiBoldFont.SetFont(editAccBtn);

            username = Intent.GetStringExtra("username");
            //sID = Intent.GetStringExtra("studID");
            //sName = Intent.GetStringExtra("studName");
            //sSchool = Intent.GetStringExtra("studSchool");
            //sCourse = Intent.GetStringExtra("studCourse");
            //sIdentity = Intent.GetStringExtra("studIdentity");


            // Set the selected item to "Account"
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_account;

            bottomNavigationView.NavigationItemSelected += BottomNavigationView_NavigationItemSelected;

            // Set the user's information

            UserInfoLogic userGetInfo = new UserInfoLogic(this);


            studInfo = userGetInfo.GetUserDetails(username);

            SetStudentInfo();


            ButtonStyler.ApplyRoundedCorners(editStudInfoBtn);
            ButtonStyler.ApplyRoundedCorners(logOutBtn);
            ButtonStyler.ApplyRoundedCorners(editAccBtn);

            editStudInfoBtn.Click += EditBtn_Click;
            logOutBtn.Click += logOutBtn_Click;
        }

        private void EditBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(editStudentInfo));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
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
            studName.Text = studInfo["studName"];
            studUsername.Text = "@" + studInfo["uname"];
            studID.Text = "Student ID: " + studInfo["studID"];
            studCourse.Text = "Course: " + studInfo["studCourse"];
            studSchool.Text = "School: " + studInfo["studSchool"];
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}