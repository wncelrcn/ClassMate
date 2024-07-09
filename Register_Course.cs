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
    public class Register_Course : AppCompatActivity
    {
        TextView title;
        Button nextBtn;
        ImageButton returnBtn;
        EditText course;
        private string username, password, studID, studName, studSchool;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.register_layout_4);

            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);

            course = FindViewById<EditText>(Resource.Id.courseTxt);

            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");
            studID = Intent.GetStringExtra("studID");
            studName = Intent.GetStringExtra("studName");
            studSchool = Intent.GetStringExtra("studSchool");


            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;


            Styler.ApplyRoundedCorners(nextBtn);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(title);
            regularFont.SetFont(course);
            semiBoldFont.SetFont(nextBtn);

        }

        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Register_School));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.PassDataToNextActivity("password", password);
            nextActivityHandler.PassDataToNextActivity("studID", studID);
            nextActivityHandler.PassDataToNextActivity("studName", studName);
            nextActivityHandler.PassDataToNextActivity("studSchool", studSchool);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            string cour = course.Text;

            if (string.IsNullOrEmpty(cour))
            {
                Toast.MakeText(this, "Please enter your course.", ToastLength.Short).Show();
                return;
            }

            else
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_Identity));
                nextActivityHandler.PassDataToNextActivity("username", username);
                nextActivityHandler.PassDataToNextActivity("password", password);
                nextActivityHandler.PassDataToNextActivity("studID", studID);
                nextActivityHandler.PassDataToNextActivity("studName", studName);
                nextActivityHandler.PassDataToNextActivity("studSchool", studSchool);
                nextActivityHandler.PassDataToNextActivity("studCourse", course.Text);
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
