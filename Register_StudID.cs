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

namespace IT123P_FinalMP
{
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register_StudID : AppCompatActivity
    {
        // widget declarations
        TextView title, desc;
        Button nextBtn;
        ImageButton returnBtn;
        EditText studID;
        private string username, password;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.register_layout_1);
            // widget initialization
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);
            desc = FindViewById<TextView>(Resource.Id.desc);
            studID = FindViewById<EditText>(Resource.Id.studIDTxt);
            title = FindViewById<TextView>(Resource.Id.title);

            // fetch data from previous activity
            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");

            // event handlers for buttons
            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;

            // Apply rounded corners to buttons
            Styler.ApplyRoundedCorners(nextBtn);

            // font styles
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(nextBtn);
            regularFont.SetFont(studID);
            boldFont.SetFont(title);
            regularFont.SetFont(desc);

        }

        // event handler for return button
        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Register));
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // event handler for next button
        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            string sID = studID.Text;

            // check if student ID is empty
            if (sID.Length == 0)
            {
                Toast.MakeText(this, "Please enter a Student ID.", ToastLength.Short).Show();
                return;
            }
            else
            {
                // check if student ID is already taken
                UserConnection userConnection = new UserConnection(this);


                if (userConnection.CheckStudID(studID.Text))
                {

                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_Name));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.PassDataToNextActivity("password", password);
                    nextActivityHandler.PassDataToNextActivity("studID", studID.Text);
                    nextActivityHandler.NavigateToNextActivity(this);
                }
                else
                {
                    Toast.MakeText(this, "Please enter a new Student ID.", ToastLength.Short).Show();
                    return;
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
