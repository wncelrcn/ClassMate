using Android.App;
using Android.Hardware.Usb;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace IT123P_FinalMP
{
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class AddNewTask : AppCompatActivity
    {
        // widget declarations
        TextView title, taskNameLbl, taskDescLbl, toDoDateLbl, dueDateLbl, classLbl;
        private EditText toDoDateTxt, dueDateTxt, taskNameTxt, taskDescTxt;
        private Button addTaskBtn;
        private ImageButton returnBtn;
        private string layoutReceiver, username, classCode, className;
        NextActivityHandler nextActivityHandler;
        UserClass userClass;
        Spinner classSpinner;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.newtask_layout);

            // fetch intents from previous activities
            layoutReceiver = Intent.GetStringExtra("layout");
            username = Intent.GetStringExtra("username");
            classCode = Intent.GetStringExtra("classCode");
            className = Intent.GetStringExtra("className");

            // widget initialization
            title = FindViewById<TextView>(Resource.Id.title);
            taskNameLbl = FindViewById<TextView>(Resource.Id.taskNameLbl);
            taskDescLbl = FindViewById<TextView>(Resource.Id.taskDescLbl);
            toDoDateLbl = FindViewById<TextView>(Resource.Id.toDoDateLbl);
            dueDateLbl = FindViewById<TextView>(Resource.Id.dueDateLbl);
            classLbl = FindViewById<TextView>(Resource.Id.classLbl);
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            addTaskBtn = FindViewById<Button>(Resource.Id.addTaskBtn);
            taskNameTxt = FindViewById<EditText>(Resource.Id.taskNameTxt);
            taskDescTxt = FindViewById<EditText>(Resource.Id.taskDescTxt);
            toDoDateTxt = FindViewById<EditText>(Resource.Id.toDoDateTxt);
            dueDateTxt = FindViewById<EditText>(Resource.Id.dueDateTxt);
            classSpinner = FindViewById<Spinner>(Resource.Id.classSpinner);

            // font styles 
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            // applying font styles to widgets
            mediumFont.SetFont(title);
            regularFont.SetFont(taskNameLbl);
            regularFont.SetFont(taskDescLbl);
            regularFont.SetFont(toDoDateLbl);
            regularFont.SetFont(dueDateLbl);
            regularFont.SetFont(classLbl);
            regularFont.SetFont(taskNameTxt);
            regularFont.SetFont(taskDescTxt);
            regularFont.SetFont(toDoDateTxt);
            regularFont.SetFont(dueDateTxt);
            semiBoldFont.SetFont(addTaskBtn);


            
            // event listeners for date pickers
            toDoDateTxt.Click += (sender, e) => ShowDatePickerDialog(toDoDateTxt);
            dueDateTxt.Click += (sender, e) => ShowDatePickerDialog(dueDateTxt);


            // user class instance
            userClass = new UserClass(this);

            // load the user's classes into the spinner
            LoadUserClasses(username);

            // button event listeners
            returnBtn.Click += ReturnBtn_Click;
            addTaskBtn.Click += AddTaskBtn_Click;

            // apply rounded corners to the buttons
            Styler.ApplyRoundedCorners(addTaskBtn);

        }

        // method to add a new task
        private void AddTaskBtn_Click(object sender, System.EventArgs e)
        {
            // check if the fields are empty
            if (string.IsNullOrEmpty(taskNameTxt.Text) || string.IsNullOrEmpty(taskDescTxt.Text) || string.IsNullOrEmpty(toDoDateTxt.Text) || string.IsNullOrEmpty(dueDateTxt.Text))
            {
                Toast.MakeText(this, "Please fill out all fields.", ToastLength.Short).Show();
                return;
            }

            // check if the spinner has a value
            if (classSpinner.SelectedItem == null || string.IsNullOrEmpty(classSpinner.SelectedItem.ToString()))
            {
                Toast.MakeText(this, "Please add a class first.", ToastLength.Short).Show();
                return;
            }

            // if all fields are valid, insert the task
            UserTask userTask = new UserTask(this);
            userTask.InsertTask(username, taskNameTxt.Text, taskDescTxt.Text, false, toDoDateTxt.Text, dueDateTxt.Text, classSpinner.SelectedItem.ToString());

            // navigate to the next activity after inserting task
            if (layoutReceiver == "dashboard")
            {
                nextActivityHandler = new NextActivityHandler(this, "", typeof(Dashboard));
            }
            else if (layoutReceiver == "classSpecific")
            {
                nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesSpecific));
                nextActivityHandler.PassDataToNextActivity("classCode", classCode);
                nextActivityHandler.PassDataToNextActivity("className", className);
            }
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // method to load the user's classes into the spinner
        private async void LoadUserClasses(string username)
        {
            await userClass.FetchAndPopulateClasses(username, classSpinner);
        }

        // method to return to previous activity
        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            if (layoutReceiver == "dashboard")
            {

                nextActivityHandler = new NextActivityHandler(this, "", typeof(Dashboard));
            }
            else if (layoutReceiver == "classSpecific")
            {
                nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesSpecific));
                nextActivityHandler.PassDataToNextActivity("classCode", classCode);
                nextActivityHandler.PassDataToNextActivity("className", className);
            }
            nextActivityHandler.PassDataToNextActivity("username", username);
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // method to show the date picker dialog
        private void ShowDatePickerDialog(EditText dateField)
        {
            // get the current date
            var calendar = Java.Util.Calendar.Instance;
            int year = calendar.Get(Java.Util.CalendarField.Year);
            int month = calendar.Get(Java.Util.CalendarField.Month);
            int day = calendar.Get(Java.Util.CalendarField.DayOfMonth);

            // create a date picker dialog
            var datePickerDialog = new DatePickerDialog(this, (sender, e) =>
            {
                string date = $"{e.Date.Day}/{e.Date.Month}/{e.Date.Year}";
                dateField.Text = date;
            }, year, month, day);

            // set the minimum date to the current date
            datePickerDialog.DatePicker.MinDate = Java.Lang.JavaSystem.CurrentTimeMillis();

            // show the date picker dialog
            datePickerDialog.Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
