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
using System;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class TaskView : AppCompatActivity
    {
        TextView taskNameTxt, taskDescTxt, toDoDateTxt, dueDateTxt, classTxt;
        ImageButton returnBtn;
        string layoutReceiver, username, classCode, className;
        Button markAsDoneBtn;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.taskview_layout);

            taskDescTxt = FindViewById<TextView>(Resource.Id.taskDesc);
            taskNameTxt = FindViewById<TextView>(Resource.Id.taskTitle);
            toDoDateTxt = FindViewById<TextView>(Resource.Id.toDoDate);
            dueDateTxt = FindViewById<TextView>(Resource.Id.dueDate);
            classTxt = FindViewById<TextView>(Resource.Id.taskClass);

            markAsDoneBtn = FindViewById<Button>(Resource.Id.markDoneBtn);

            layoutReceiver = Intent.GetStringExtra("layout");

            username = Intent.GetStringExtra("username");
            classCode = Intent.GetStringExtra("classCode");
            className = Intent.GetStringExtra("className");


            taskNameTxt.Text = Intent.GetStringExtra("taskName");
            taskDescTxt.Text = Intent.GetStringExtra("taskDesc");

            toDoDateTxt.Text = $"To do Date: {Intent.GetStringExtra("toDoDate")}";
            dueDateTxt.Text = $"Due Date: {Intent.GetStringExtra("dueDate")}";
           
            classTxt.Text = $"Class: {Intent.GetStringExtra("taskClass")}";

            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            returnBtn.Click += ReturnBtn_Click;

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(taskNameTxt);
            mediumFont.SetFont(taskDescTxt);
            regularFont.SetFont(toDoDateTxt);
            regularFont.SetFont(dueDateTxt);
            regularFont.SetFont(classTxt);
            semiBoldFont.SetFont(markAsDoneBtn);

            ButtonStyler.ApplyRoundedCorners(markAsDoneBtn);
        }

        public void ReturnBtn_Click(object sender, EventArgs e)
        {
            if (layoutReceiver == "class")
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesSpecific));
                nextActivityHandler.PassDataToNextActivity("classCode", Intent.GetStringExtra("classCode"));
                nextActivityHandler.PassDataToNextActivity("className", Intent.GetStringExtra("className"));
                nextActivityHandler.PassDataToNextActivity("username", Intent.GetStringExtra("username"));
                nextActivityHandler.NavigateToNextActivity(this);
            }
            else if (layoutReceiver == "dashboard")
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", Intent.GetStringExtra("username"));

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
