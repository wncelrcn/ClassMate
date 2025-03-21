﻿using Android.App;
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
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ViewAccount : AppCompatActivity
    {
        // widget declarations
        BottomNavigationView bottomNavigationView;
        TextView title, studName, studUsername, studID, studCourse, studSchool;
        Button editStudInfoBtn, logOutBtn, editAccBtn;
        ImageView profilePic;


        string username, sID, sName, sSchool, sCourse, sIdentity;
        Dictionary<string, string> studInfo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
           
            SetContentView(Resource.Layout.account_layout);

            // Widgets Initialization
            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);
            
            studName = FindViewById<TextView>(Resource.Id.studNameTxt);
            studUsername = FindViewById<TextView>(Resource.Id.studUsernameTxt);
            studID = FindViewById<TextView>(Resource.Id.studIDTxt);
            studCourse = FindViewById<TextView>(Resource.Id.studCourseTxt);
            studSchool = FindViewById<TextView>(Resource.Id.studSchoolTxt);
            editStudInfoBtn = FindViewById<Button>(Resource.Id.editStudInfoBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            editAccBtn = FindViewById<Button>(Resource.Id.editAccBtn);
            logOutBtn = FindViewById<Button>(Resource.Id.logOutBtn);
            profilePic = FindViewById<ImageView>(Resource.Id.profilePic);

            // Set Fonts
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(title);
            semiBoldFont.SetFont(studName);
            regularFont.SetFont(studUsername);
            regularFont.SetFont(studID);
            regularFont.SetFont(studCourse);
            regularFont.SetFont(studSchool);
            semiBoldFont.SetFont(editStudInfoBtn);
            semiBoldFont.SetFont(logOutBtn);
            semiBoldFont.SetFont(editAccBtn);

            // Get Intent Data
            username = Intent.GetStringExtra("username");

            // Set the Bottom Navigation View
            BottomNavigationViewLogic bottomNav = new BottomNavigationViewLogic(this, bottomNavigationView, username, "ViewAccount");
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_tasks;
            bottomNavigationView.NavigationItemSelected += bottomNav.BottomNavigationView_NavigationItemSelected;
            bottomNav.SetInitialSelectedItem("ViewAccount");

            // Set the user's information
            UserInfoLogic userGetInfo = new UserInfoLogic(this);
            studInfo = userGetInfo.GetUserDetails(username);
            SetStudentInfo();

            // Button Styling
            Styler.ApplyRoundedCorners(editStudInfoBtn);
            Styler.ApplyRoundedCorners(logOutBtn);
            Styler.ApplyRoundedCorners(editAccBtn);

            // Button Click Events
            editStudInfoBtn.Click += EditBtn_Click;
            logOutBtn.Click += logOutBtn_Click;
            editAccBtn.Click += editAccBtn_Click;
        }

        // Button Click Event for Edit Account Credentials
        private void editAccBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(EditAccCredentials));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // Button Click Event for Edit Student Information
        private void EditBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(EditStudentInfo));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // Button Click Event for Log Out
        public void logOutBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Landing));
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // Set the student's information to the text fields
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