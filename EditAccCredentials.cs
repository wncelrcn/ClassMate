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
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class EditAccCredentials : AppCompatActivity
    {
        // widget declarations
        Button updateBtn;
        ImageButton returnBtn;
        EditText usernameTxt, oldpasswordTxt, newpasswordTxt, confirmpasswordTxt;
        TextView title, usernameLbl, passwordLbl;
        string username;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.editaccdetails_layout);

            // fetch username from previous activity
            username = Intent.GetStringExtra("username");

            // widget initialization
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            updateBtn = FindViewById<Button>(Resource.Id.updateBtn);
            usernameLbl = FindViewById<TextView>(Resource.Id.usernameLbl);
            passwordLbl = FindViewById<TextView>(Resource.Id.passLbl);
            usernameTxt = FindViewById<EditText>(Resource.Id.newUnameTxt);
            oldpasswordTxt = FindViewById<EditText>(Resource.Id.currentPassTxt);
            newpasswordTxt = FindViewById<EditText>(Resource.Id.newPassTxt);
            confirmpasswordTxt = FindViewById<EditText>(Resource.Id.confirmPassTxt);
            title = FindViewById<TextView>(Resource.Id.title);

            // set username to usernameTxt
            usernameTxt.Text = username;

            // font styles
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

            // apply rounded corners to buttons
            Styler.ApplyRoundedCorners(updateBtn);
            
            // event handlers for buttons
            returnBtn.Click += ReturnBtn_Click;
            updateBtn.Click += UpdateBtn_Click;
        }

        // update button event handler
        private void UpdateBtn_Click(object sender, EventArgs e)
        {
            // check if fields are empty
            if (string.IsNullOrEmpty(oldpasswordTxt.Text) ||
                string.IsNullOrEmpty(newpasswordTxt.Text) ||
                string.IsNullOrEmpty(confirmpasswordTxt.Text))
            {
                Toast.MakeText(this, "Please fill up all fields.", ToastLength.Short).Show();
            }
            
            // check if new password and confirm password are not the same
            else if (newpasswordTxt.Text != confirmpasswordTxt.Text)
            {
                Toast.MakeText(this, "Passwords do not match.", ToastLength.Short).Show();
            }

            // update password
            else
            {
                // instantiate UserConnection class
                UserConnection userConnection = new UserConnection(this);
                bool isUpdated = userConnection.UpdatePassword(username, oldpasswordTxt.Text, newpasswordTxt.Text);

                // check if password is updated
                if (isUpdated)
                {
                    // show toast message and clear fields
                    Toast.MakeText(this, "Password updated", ToastLength.Short).Show();
                    oldpasswordTxt.Text = "";
                    newpasswordTxt.Text = "";
                    confirmpasswordTxt.Text = "";

                    // navigate to ViewAccount activity
                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ViewAccount));
                    nextActivityHandler.PassDataToNextActivity("username", username);
                    nextActivityHandler.NavigateToNextActivity(this);
                }

                // show toast message if old password is incorrect
                else
                {
                    Toast.MakeText(this, "Your old password is incorrect.", ToastLength.Short).Show();
                }
            }
        }


        // return button event handler
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