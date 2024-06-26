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
    public class Register_Identity : AppCompatActivity
    {
        TextView title;
        Button returnBtn, nextBtn;
        RadioGroup identityGroup;
        RadioButton femaleRadBtn, maleRadBtn, noRadBtn;
        private string username, password, studID, studName, studSchool, studCourse;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.register_layout_5);

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);

            identityGroup = FindViewById<RadioGroup>(Resource.Id.identityGroup);
            femaleRadBtn = FindViewById<RadioButton>(Resource.Id.femaleRadGrp);
            maleRadBtn = FindViewById<RadioButton>(Resource.Id.maleRadGrp);
            noRadBtn = FindViewById<RadioButton>(Resource.Id.noRadGrp);

            username = Intent.GetStringExtra("username");
            password = Intent.GetStringExtra("password");
            studID = Intent.GetStringExtra("studID");
            studName = Intent.GetStringExtra("studName");
            studSchool = Intent.GetStringExtra("studSchool");
            studCourse = Intent.GetStringExtra("studCourse");

            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;

            ButtonStyler.ApplyRoundedCorners(nextBtn);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(title);
            semiBoldFont.SetFont(nextBtn);
            mediumFont.SetFont(femaleRadBtn);
            mediumFont.SetFont(maleRadBtn);
            mediumFont.SetFont(noRadBtn);
        }

        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Register_Course));
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
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_PrivatePolicy));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.PassDataToNextActivity("password", password);
            nextActivityHandler.PassDataToNextActivity("studID", studID);
            nextActivityHandler.PassDataToNextActivity("studName", studName);
            nextActivityHandler.PassDataToNextActivity("studSchool", studSchool);
            nextActivityHandler.PassDataToNextActivity("studCourse", studCourse);

            int selectedId = identityGroup.CheckedRadioButtonId;
            RadioButton selectedRadioButton = FindViewById<RadioButton>(selectedId);
            string selectedText = selectedRadioButton.Text;


            nextActivityHandler.PassDataToNextActivity("studIdentity", selectedText);

            nextActivityHandler.NavigateToNextActivity(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
