using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Graphics;
using System;


namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Landing : AppCompatActivity
    {
        Button signUpBtn, loginBtn;
        TextView landingTitle1, landingTitle2;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.landing_layout);

            signUpBtn = FindViewById<Button>(Resource.Id.signUpBtn);
            loginBtn = FindViewById<Button>(Resource.Id.loginBtn);
            landingTitle1 = FindViewById<TextView>(Resource.Id.landingTitle1);
            landingTitle2 = FindViewById<TextView>(Resource.Id.landingTitle2);

            signUpBtn.Click += SignUpBtn_Click;
            loginBtn.Click += LoginBtn_Click;

            // Apply rounded corners to buttons
            ButtonStyler.ApplyRoundedCorners(signUpBtn);
            ButtonStyler.ApplyRoundedCorners(loginBtn);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            mediumFont.SetFont(landingTitle1);
            mediumFont.SetFont(landingTitle2);
            semiBoldFont.SetFont(loginBtn);
            semiBoldFont.SetFont(signUpBtn);
        }

        public void SignUpBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Register));
            nextActivityHandler.NavigateToNextActivity(this);
        }

        public void LoginBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Login));
            nextActivityHandler.NavigateToNextActivity(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
