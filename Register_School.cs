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
    public class Register_School : AppCompatActivity
    {
        // widget declarations
        TextView title;
        Button nextBtn;
        ImageButton returnBtn;
        EditText school;
        private string username, password, studID, studName;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.register_layout_3);

            // widget initialization
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);

            school = FindViewById<EditText>(Resource.Id.schoolTxt);

            // fetch data from previous activity
            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");
            studID = Intent.GetStringExtra("studID");
            studName = Intent.GetStringExtra("studName");

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
            regularFont.SetFont(school);
            semiBoldFont.SetFont(nextBtn);

        }

        // return button click event
        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Register_Name));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.PassDataToNextActivity("password", password);
            nextActivityHandler.PassDataToNextActivity("studID", studID);
            nextActivityHandler.PassDataToNextActivity("studName", studName);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // next button click event
        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            string sch = school.Text;

            // check if school is empty
            if (string.IsNullOrEmpty(sch))
            {

                Toast.MakeText(this, "Please enter your school.", ToastLength.Short).Show();
                return;
            }
            // pass data to next activity
            else
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_Course));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.PassDataToNextActivity("password", password);
                nextActivityHandler.PassDataToNextActivity("studID", studID);
                nextActivityHandler.PassDataToNextActivity("studName", studName);
                nextActivityHandler.PassDataToNextActivity("studSchool", school.Text);
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
