using Android.App;
using Android.Hardware.Usb;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClassesAddView : AppCompatActivity
    {

        TextView title, codeLbl, nameLbl;
        Button addClassBtn;
        ImageButton returnBtn;
        EditText classCodeTxt, classNameTxt;
        string username;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.classes_new_layout);

            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            addClassBtn = FindViewById<Button>(Resource.Id.addClassBtn);
            title = FindViewById<TextView>(Resource.Id.title);
            codeLbl = FindViewById<TextView>(Resource.Id.classCodeLbl);
            nameLbl = FindViewById<TextView>(Resource.Id.classNameLbl);

            addClassBtn.Click += AddClassBtn_Click;

            classCodeTxt = FindViewById<EditText>(Resource.Id.classCodeTxt);
            classNameTxt = FindViewById<EditText>(Resource.Id.classNameTxt);

            returnBtn.Click += ReturnBtn_Click;

            username = Intent.GetStringExtra("username");

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(title);
            regularFont.SetFont(codeLbl);
            regularFont.SetFont(nameLbl);
            regularFont.SetFont(classCodeTxt);
            regularFont.SetFont(classNameTxt);
            semiBoldFont.SetFont(addClassBtn);

            Styler.ApplyRoundedCorners(addClassBtn);

        }

        private void ReturnBtn_Click(object sender, EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesMainView));
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        private void AddClassBtn_Click(object sender, EventArgs e)
        {
            UserClass userClass = new UserClass(this);
            userClass.UserAddClass(username, classCodeTxt.Text, classNameTxt.Text);

            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesMainView));
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