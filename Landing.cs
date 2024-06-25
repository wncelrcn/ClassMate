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
using Mod3RESTTask;
using Android.Graphics;
using System.Runtime.Remoting.Contexts;
using System;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Landing : AppCompatActivity
    {
        Button signUpBtn, loginBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.landing_layout);

            signUpBtn = FindViewById<Button>(Resource.Id.signUpBtn);
            loginBtn = FindViewById<Button>(Resource.Id.loginBtn);

            signUpBtn.Click += SignUpBtn_Click;
            loginBtn.Click += LoginBtn_Click;

        }

        public void SignUpBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Register));
            nextActivityHandler.NavigateToNextActivity();
        }

        public void LoginBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Login));
            nextActivityHandler.NavigateToNextActivity();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
