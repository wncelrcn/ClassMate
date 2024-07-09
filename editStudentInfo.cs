using Android.App;
using Android.Hardware.Usb;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class EditStudentInfo : AppCompatActivity
    {
        TextView title, nameLbl, studIDLbl, courseLbl, schoolLbl;
        Button updateBtn;
        ImageButton returnBtn;
        EditText nameTxt, studIdTxt, courseTxt, schoolTxt;
        string username;
        Dictionary<string, string> studInfo;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.editstudentinfo_layout);

            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            updateBtn = FindViewById<Button>(Resource.Id.updateBtn);

            title = FindViewById<TextView>(Resource.Id.title);
            nameLbl = FindViewById<TextView>(Resource.Id.nameLbl);
            studIDLbl = FindViewById<TextView>(Resource.Id.studIDLbl);
            courseLbl = FindViewById<TextView>(Resource.Id.courseLbl);
            schoolLbl = FindViewById<TextView>(Resource.Id.schoolLbl);


            nameTxt = FindViewById<EditText>(Resource.Id.newNameTxt);
            studIdTxt = FindViewById<EditText>(Resource.Id.newStudIDTxt);
            courseTxt = FindViewById<EditText>(Resource.Id.newCourseTxt);
            schoolTxt = FindViewById<EditText>(Resource.Id.newSchoolTxt);

            UserInfoLogic userGetInfo = new UserInfoLogic(this);

            username = Intent.GetStringExtra("username");

            studInfo = userGetInfo.GetUserDetails(username);


            nameTxt.Text = studInfo["studName"];
            studIdTxt.Text = studInfo["studID"];
            courseTxt.Text = studInfo["studCourse"];
            schoolTxt.Text = studInfo["studSchool"];

            Styler.ApplyRoundedCorners(updateBtn);

            returnBtn.Click += ReturnBtn_Click;
            updateBtn.Click += UpdateBtn_Click;

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(title);
            regularFont.SetFont(nameLbl);
            regularFont.SetFont(studIDLbl);
            regularFont.SetFont(courseLbl);
            regularFont.SetFont(schoolLbl);
            regularFont.SetFont(nameTxt);
            regularFont.SetFont(studIdTxt);
            regularFont.SetFont(courseTxt);
            regularFont.SetFont(schoolTxt);
            semiBoldFont.SetFont(updateBtn);

        }

        private void ReturnBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ViewAccount));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }


        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            if (nameTxt.Text == "" || studIdTxt.Text == "" || courseTxt.Text == "" || schoolTxt.Text == "")
            {
                Toast.MakeText(this, "Please fill up all fields.", ToastLength.Short).Show();
                return;
            }

            else
            {
                UserInfoLogic updateInfo = new UserInfoLogic(this);

                updateInfo.UpdateStudentInfo(nameTxt.Text, studIdTxt.Text, courseTxt.Text, schoolTxt.Text, username);
            }
        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}