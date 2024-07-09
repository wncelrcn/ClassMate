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
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClassesSpecific : AppCompatActivity
    {
        // widget declarations
        ImageButton returnBtn, deleteBtn;
        TextView classCodeTxt, classNameTxt, title;
        string classCode, className, username;
        LinearLayout TaskViewContainer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.classes_specific_layout);

            // fetch data from previous activities
            classCode = Intent.GetStringExtra("classCode");
            className = Intent.GetStringExtra("className");
            username = Intent.GetStringExtra("username");

            // widget initialization
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            deleteBtn = FindViewById<ImageButton>(Resource.Id.deleteClassBtn);
            classCodeTxt = FindViewById<TextView>(Resource.Id.classCodeTxt);
            classNameTxt = FindViewById<TextView>(Resource.Id.classNameTxt);
            title = FindViewById<TextView>(Resource.Id.title);
            TaskViewContainer = FindViewById<LinearLayout>(Resource.Id.taskViewContainer);
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);

            // font styles
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(classCodeTxt);
            regularFont.SetFont(classNameTxt);
            semiBoldFont.SetFont(title);
            
            // set the text of the textviews
            classCodeTxt.Text = classCode;
            classNameTxt.Text = className;

            // event handlers for the buttons
            deleteBtn.Click += DeleteBtn_Click;
            returnBtn.Click += ReturnBtn_Click;


            // event handler for the floating action button
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
            
            // load the tasks
            LoadTasks(username, classCode, className);
        }

        // method to load the tasks
        private async void LoadTasks(string username, string classCode, string className)
        {
            // instantiate the UserTask class
            var taskHandler = new UserTask(this, TaskViewContainer);
            await taskHandler.GetTaskPerClass(username, classCode, className);

        }

        // event handler for the return button
        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivity = new NextActivityHandler(this, "", typeof(ClassesMainView));
            nextActivity.PassDataToNextActivity("username", username);
            nextActivity.NavigateToNextActivity(this);
        }

        // event handler for the delete button
        public void DeleteBtn_Click(object sender, System.EventArgs e)
        {
            DialogButtonClickHandler onOkayClick = () =>
            {
                UserClass userClass = new UserClass(this);
                userClass.UserDeleteClass(username, classCode);

                return true;
            };

            DialogButtonClickHandler onCancelClick = () =>
            {
                return false;
            };

            custom_dialog dialog = new custom_dialog(this, "Are you sure you want to delete this class?", onOkayClick, onCancelClick);
            dialog.Show();
        }
 




        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
