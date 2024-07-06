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
using Android.Graphics;
using Android.Media;
using Google.Android.Material.FloatingActionButton;
using System;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClassesSpecific : AppCompatActivity
    {

        ImageButton returnBtn, deleteBtn;
        TextView classCodeTxt, classNameTxt, title;
        string classCode, className, username;
        LinearLayout TaskViewContainer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.classes_specific_layout);

            classCode = Intent.GetStringExtra("classCode");
            className = Intent.GetStringExtra("className");
            username = Intent.GetStringExtra("username");

            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            deleteBtn = FindViewById<ImageButton>(Resource.Id.deleteClassBtn);
            classCodeTxt = FindViewById<TextView>(Resource.Id.classCodeTxt);
            classNameTxt = FindViewById<TextView>(Resource.Id.classNameTxt);
            title = FindViewById<TextView>(Resource.Id.title);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(classCodeTxt);
            regularFont.SetFont(classNameTxt);
            semiBoldFont.SetFont(title);

            TaskViewContainer = FindViewById<LinearLayout>(Resource.Id.taskViewContainer);

            
            classCodeTxt.Text = classCode;
            classNameTxt.Text = className;
            deleteBtn.Click += DeleteBtn_Click;
            returnBtn.Click += ReturnBtn_Click;


            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += (sender, args) =>
            {
                // Handle the click event, e.g., open a new task creation screen
                NextActivityHandler nextActivity = new NextActivityHandler(this, "", typeof(AddNewTask));
                nextActivity.PassDataToNextActivity("username", username);

                nextActivity.PassDataToNextActivity("layout", "classSpecific");
                nextActivity.PassDataToNextActivity("classCode", classCode);
                nextActivity.PassDataToNextActivity("className", className);
                nextActivity.NavigateToNextActivity(this);
            };

            LoadTasks(username, classCode, className);
        }

        private async void LoadTasks(string username, string classCode, string className)
        {

            var taskHandler = new UserTask(this, TaskViewContainer);
            await taskHandler.GetTaskPerClass(username, classCode, className);

        }


        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivity = new NextActivityHandler(this, "", typeof(ClassesMainView));
            nextActivity.PassDataToNextActivity("username", username);
            nextActivity.NavigateToNextActivity(this);
        }

        public void DeleteBtn_Click(object sender, System.EventArgs e)
        {
            UserClass userClass = new UserClass(this);
            userClass.UserDeleteClass(username, classCode);


        }



        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
