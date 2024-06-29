using Android.App;
using Android.Hardware.Usb;
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
        private EditText toDoDateTxt, dueDateTxt, taskNameTxt, taskDescTxt;
        private Button returnBtn, addTaskBtn;
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

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            addTaskBtn = FindViewById<Button>(Resource.Id.addTaskBtn);

            taskNameTxt = FindViewById<EditText>(Resource.Id.taskNameTxt);
            taskDescTxt = FindViewById<EditText>(Resource.Id.taskDescTxt);
            toDoDateTxt = FindViewById<EditText>(Resource.Id.toDoDateTxt);
            dueDateTxt = FindViewById<EditText>(Resource.Id.dueDateTxt);



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

        }

        private void AddTaskBtn_Click(object sender, System.EventArgs e)
        {
       
           
                UserTask userTask = new UserTask(this);
                userTask.InsertTask(username, taskNameTxt.Text, taskDescTxt.Text, false, toDoDateTxt.Text, dueDateTxt.Text, classSpinner.SelectedItem.ToString());
          

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
