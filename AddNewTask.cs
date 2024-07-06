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
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class AddNewTask : AppCompatActivity
    {
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
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.newtask_layout);

            layoutReceiver = Intent.GetStringExtra("layout");
            username = Intent.GetStringExtra("username");
            classCode = Intent.GetStringExtra("classCode");
            className = Intent.GetStringExtra("className");
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



            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

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

            classSpinner = FindViewById<Spinner>(Resource.Id.classSpinner);

            toDoDateTxt.Click += (sender, e) => ShowDatePickerDialog(toDoDateTxt);
            dueDateTxt.Click += (sender, e) => ShowDatePickerDialog(dueDateTxt);


            // Find views by ID
            classSpinner = FindViewById<Spinner>(Resource.Id.classSpinner);
            userClass = new UserClass(this);


            // Load the user's classes into the Spinner
            LoadUserClasses(username);

            returnBtn.Click += ReturnBtn_Click;
            addTaskBtn.Click += AddTaskBtn_Click;

            ButtonStyler.ApplyRoundedCorners(addTaskBtn);

        }

        private void AddTaskBtn_Click(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(taskNameTxt.Text) || string.IsNullOrEmpty(taskDescTxt.Text) || string.IsNullOrEmpty(toDoDateTxt.Text) || string.IsNullOrEmpty(dueDateTxt.Text))
            {
                Toast.MakeText(this, "Please fill out all fields.", ToastLength.Short).Show();
                return;
            }

            // Check if the spinner has a value
            if (classSpinner.SelectedItem == null || string.IsNullOrEmpty(classSpinner.SelectedItem.ToString()))
            {
                Toast.MakeText(this, "Please add a class first.", ToastLength.Short).Show();
                return;
            }

            // If all fields are valid, insert the task
            UserTask userTask = new UserTask(this);
            userTask.InsertTask(username, taskNameTxt.Text, taskDescTxt.Text, false, toDoDateTxt.Text, dueDateTxt.Text, classSpinner.SelectedItem.ToString());

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


        private async void LoadUserClasses(string username)
        {
            await userClass.FetchAndPopulateClasses(username, classSpinner);
        }

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


        private void ShowDatePickerDialog(EditText dateField)
        {
            var calendar = Java.Util.Calendar.Instance;
            int year = calendar.Get(Java.Util.CalendarField.Year);
            int month = calendar.Get(Java.Util.CalendarField.Month);
            int day = calendar.Get(Java.Util.CalendarField.DayOfMonth);

            var datePickerDialog = new DatePickerDialog(this, (sender, e) =>
            {
                string date = $"{e.Date.Day}/{e.Date.Month}/{e.Date.Year}";
                dateField.Text = date;
            }, year, month, day);

            // Set the minimum date to the current date
            datePickerDialog.DatePicker.MinDate = Java.Lang.JavaSystem.CurrentTimeMillis();

            datePickerDialog.Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
