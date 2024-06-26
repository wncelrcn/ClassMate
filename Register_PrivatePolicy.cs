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

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register_PrivatePolicy : AppCompatActivity
    {
        TextView title, desc;
        Button returnBtn, nextBtn;
        CheckBox checkBox;
        private string username, password, studID, studName, studSchool, studCourse, studIdentity;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.register_layout_6);

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);
            desc = FindViewById<TextView>(Resource.Id.desc);
            checkBox = FindViewById<CheckBox>(Resource.Id.agreeCheckBox);

            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");
            studID = Intent.GetStringExtra("studID");
            studName = Intent.GetStringExtra("studName");
            studSchool = Intent.GetStringExtra("studSchool");
            studCourse = Intent.GetStringExtra("studCourse");
            studIdentity = Intent.GetStringExtra("studIdentity");

            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;

            ButtonStyler.ApplyRoundedCorners(nextBtn);


            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(title);
            regularFont.SetFont(desc);
            semiBoldFont.SetFont(nextBtn);
            regularFont.SetFont(checkBox);
        }

        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Register_Identity));

            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.PassDataToNextActivity("password", password);
            nextActivityHandler.PassDataToNextActivity("studID", studID);
            nextActivityHandler.PassDataToNextActivity("studName", studName);
            nextActivityHandler.PassDataToNextActivity("studSchool", studSchool);
            nextActivityHandler.PassDataToNextActivity("studCourse", studCourse);

            nextActivityHandler.NavigateToNextActivity(this);
        }

        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            if (!checkBox.Checked)
            {

                Toast.MakeText(this, "Please agree to the privacy policy before proceeding.", ToastLength.Short).Show();
            }
            else
            {

                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Dashboard));

                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.PassDataToNextActivity("password", password);
                nextActivityHandler.PassDataToNextActivity("studID", studID);
                nextActivityHandler.PassDataToNextActivity("studName", studName);
                nextActivityHandler.PassDataToNextActivity("studSchool", studSchool);
                nextActivityHandler.PassDataToNextActivity("studCourse", studCourse);
                nextActivityHandler.PassDataToNextActivity("studIdentity", studIdentity);

                UserConnection userConnection = new UserConnection(this);
                userConnection.Register(username, password, studID, studName, studSchool, studCourse, studIdentity);

                nextActivityHandler.NavigateToNextActivity(this);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
