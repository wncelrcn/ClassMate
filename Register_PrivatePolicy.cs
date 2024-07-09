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
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register_PrivatePolicy : AppCompatActivity
    {
        // widget declarations
        TextView title, desc;
        Button nextBtn;
        ImageButton returnBtn;
        CheckBox checkBox;
        private string username, password, studID, studName, studSchool, studCourse, studIdentity;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.register_layout_6);

            // widget initialization
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);
            desc = FindViewById<TextView>(Resource.Id.desc);
            checkBox = FindViewById<CheckBox>(Resource.Id.agreeCheckBox);

            // fetch data from previous activity
            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");
            studID = Intent.GetStringExtra("studID");
            studName = Intent.GetStringExtra("studName");
            studSchool = Intent.GetStringExtra("studSchool");
            studCourse = Intent.GetStringExtra("studCourse");
            studIdentity = Intent.GetStringExtra("studIdentity");

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

            boldFont.SetFont(title);
            regularFont.SetFont(desc);
            semiBoldFont.SetFont(nextBtn);
            regularFont.SetFont(checkBox);
        }

        // return button click event
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

        // next button click event
        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            // check if user agreed to the privacy policy
            if (!checkBox.Checked)
            {

                Toast.MakeText(this, "Please agree to the privacy policy before proceeding.", ToastLength.Short).Show();
            }
            else
            {
                // register user
                UserConnection userConnection = new UserConnection(this);
                userConnection.Register(username, password, studID, studName, studSchool, studCourse, studIdentity);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
