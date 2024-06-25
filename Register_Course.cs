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
using Mod3RESTTask;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register_Course : AppCompatActivity
    {

        Button returnBtn, nextBtn;
        EditText course;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.register_layout_4);

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);

            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);

            course = FindViewById<EditText>(Resource.Id.courseTxt);

            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;



        }

        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Register_School));
            nextActivityHandler.NavigateToNextActivity();
        }

        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_Identity));
            nextActivityHandler.NavigateToNextActivity();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
