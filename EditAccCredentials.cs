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
    public class EditAccCredentials : AppCompatActivity
    {
        Button returnBtn, updateBtn;
        EditText usernameTxt, oldpasswordTxt, newpasswordTxt, confirmpasswordTxt;
        TextView title, usernameLbl, passwordLbl;
        string username;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.editaccdetails_layout);

            username = Intent.GetStringExtra("username");

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            updateBtn = FindViewById<Button>(Resource.Id.updateBtn);

            usernameLbl = FindViewById<TextView>(Resource.Id.usernameLbl);
            passwordLbl = FindViewById<TextView>(Resource.Id.passLbl);

            usernameTxt = FindViewById<EditText>(Resource.Id.newUnameTxt);
            oldpasswordTxt = FindViewById<EditText>(Resource.Id.currentPassTxt);
            newpasswordTxt = FindViewById<EditText>(Resource.Id.newPassTxt);
            confirmpasswordTxt = FindViewById<EditText>(Resource.Id.confirmPassTxt);

            title = FindViewById<TextView>(Resource.Id.title);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(title);
            regularFont.SetFont(usernameLbl);
            regularFont.SetFont(passwordLbl);
            regularFont.SetFont(usernameTxt);
            regularFont.SetFont(oldpasswordTxt);
            regularFont.SetFont(newpasswordTxt);
            regularFont.SetFont(confirmpasswordTxt);
            semiBoldFont.SetFont(updateBtn);

            ButtonStyler.ApplyRoundedCorners(updateBtn);
            returnBtn.Click += ReturnBtn_Click;
        }

        private void ReturnBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ViewAccount));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}