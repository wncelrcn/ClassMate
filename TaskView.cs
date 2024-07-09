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
using AndroidX.Annotations;


namespace IT123P_FinalMP
{
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class TaskView : AppCompatActivity
    {

        // widget declarations
        TextView taskNameTxt, taskDescTxt, toDoDateTxt, dueDateTxt, classTxt, separator, timerLabel;
        ImageButton returnBtn, deleteBtn;
        Button markAsDoneBtn, startStopButton, resetBtn;
        EditText timerMinuteText, timerSecondText;
        LinearLayout layoutPomodoro;
        string layoutReceiver, username, classCode, className, tN, cC;
        private bool isRunning = false;
        private PomodoroLogic pomodoroLogic;
        


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.taskview_layout);
            // widget initialization
            taskDescTxt = FindViewById<TextView>(Resource.Id.taskDesc);
            taskNameTxt = FindViewById<TextView>(Resource.Id.taskTitle);
            toDoDateTxt = FindViewById<TextView>(Resource.Id.toDoDate);
            dueDateTxt = FindViewById<TextView>(Resource.Id.dueDate);
            classTxt = FindViewById<TextView>(Resource.Id.taskClass);
            separator = FindViewById<TextView>(Resource.Id.separator);
            markAsDoneBtn = FindViewById<Button>(Resource.Id.markDoneBtn);
            startStopButton = FindViewById<Button>(Resource.Id.startStopButton);
            resetBtn = FindViewById<Button>(Resource.Id.resetButton);
            deleteBtn = FindViewById<ImageButton>(Resource.Id.deleteTaskBtn);
            timerMinuteText = FindViewById<EditText>(Resource.Id.timerMinuteText);
            timerSecondText = FindViewById<EditText>(Resource.Id.timerSecondText);
            layoutPomodoro = FindViewById<LinearLayout>(Resource.Id.layoutPomodoro);
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            timerLabel = FindViewById<TextView>(Resource.Id.timerLabel);
            Styler.ApplyRoundedCorners(layoutPomodoro, Color.ParseColor("#DDEDEA"));

            // fetch data from previous activity
            layoutReceiver = Intent.GetStringExtra("layout");
            username = Intent.GetStringExtra("username");
            classCode = Intent.GetStringExtra("classCode");
            className = Intent.GetStringExtra("className");

            tN = Intent.GetStringExtra("taskName");
            cC = Intent.GetStringExtra("taskClass");

            // set data to widgets
            taskNameTxt.Text = Intent.GetStringExtra("taskName");
            taskDescTxt.Text = Intent.GetStringExtra("taskDesc");
            toDoDateTxt.Text = $"To do Date: {Intent.GetStringExtra("toDoDate")}";
            dueDateTxt.Text = $"Due Date: {Intent.GetStringExtra("dueDate")}";
            classTxt.Text = $"Class: {Intent.GetStringExtra("taskClass")}";

           
            
            // event handlers for buttons
            returnBtn.Click += ReturnBtn_Click;
            markAsDoneBtn.Click += MarkAsDoneBtn_Click;
            deleteBtn.Click += DeleteBtn_Click;

            // font styles
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
            semiBoldFont.SetFont(startStopButton);
            semiBoldFont.SetFont(resetBtn);
            semiBoldFont.SetFont(timerLabel);
            mediumFont.SetFont(separator);
            mediumFont.SetFont(timerMinuteText);
            mediumFont.SetFont(timerSecondText);
            Styler.ApplyRoundedCorners(markAsDoneBtn);

            // Pomodoro Timer
            timerMinuteText = FindViewById<EditText>(Resource.Id.timerMinuteText);
            timerSecondText = FindViewById<EditText>(Resource.Id.timerSecondText);

            // Set filters for input to limit to 2 digits only (00:00)
            timerMinuteText.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(2) });
            timerSecondText.SetFilters(new IInputFilter[] { new InputFilterLengthFilter(2) });

            // Event handlers for input validation
            timerMinuteText.TextChanged += (sender, e) =>
            {
                // Validate input
                ValidateInput(sender as EditText, e);
            };


            timerSecondText.TextChanged += (sender, e) =>
            {
                ValidateInput(sender as EditText, e);
            };

            // Event handler for start/stop button
            startStopButton.Click += PomodoroBtn_Click;
            resetBtn.Click += ResetTimer_Click;

        }

        // Event handler for reset button
        public void ResetTimer_Click(object sender, EventArgs e)
        {
            // Reset timer to 00:00
            timerMinuteText.Text = "00";
            timerSecondText.Text = "00";
            isRunning = false;
            startStopButton.Text = "Start";
            pomodoroLogic?.StopPomodoro();
        }

        // Event handler for start/stop button
        public void PomodoroBtn_Click(object sender, EventArgs e)
        {
            // Check if timer is set to 00:00
            if (timerMinuteText.Text == "00" && timerSecondText.Text == "00")
            {
                Toast.MakeText(this, "Please input a valid time.", ToastLength.Short).Show();
                startStopButton.Text = "Start";
                isRunning = false;

                return;
            }

            // Start/Stop timer
            if (isRunning)
            {
                pomodoroLogic?.StopPomodoro();
                startStopButton.Text = "Start";
                isRunning = false;
            }
            else
            {
                string minute = timerMinuteText.Text;
                string secs = timerSecondText.Text;
                pomodoroLogic = new PomodoroLogic(this, minute, secs, timerMinuteText, timerSecondText, startStopButton);
                pomodoroLogic.StartPomodoro();
                startStopButton.Text = "Stop";
                isRunning = true;
            }
        }
    



    // Validate input for timer
    private void ValidateInput(EditText editText, TextChangedEventArgs e)
        {
            // Get input from EditText
            string input = editText.Text;
            if (input.Length > 2)
            {
                // Limit input to 2 characters
                editText.Text = input.Substring(0, 2);
                editText.SetSelection(editText.Text.Length); // Move cursor to the end
            }
            else
            {
                // Check if input is not a digit
                foreach (char c in input)
                {
                    // Remove non-digit characters
                    if (!char.IsDigit(c))
                    {
                        // Remove non-digit character
                        editText.Text = input.Remove(input.IndexOf(c), 1);
                        editText.SetSelection(editText.Text.Length); // Move cursor to the end
                        break;
                    }
                }
            }
        }
    
    // Event handler for return button
    public void ReturnBtn_Click(object sender, EventArgs e)
        {
            // Return to ClassesSpecific view if user came from there
            if (layoutReceiver == "class")
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesSpecific));
                nextActivityHandler.PassDataToNextActivity("classCode", Intent.GetStringExtra("classCode"));
                nextActivityHandler.PassDataToNextActivity("className", Intent.GetStringExtra("className"));
                nextActivityHandler.PassDataToNextActivity("username", Intent.GetStringExtra("username"));
                nextActivityHandler.NavigateToNextActivity(this);
            }
            // Return to Dashboard if user came from there
            else if (layoutReceiver == "dashboard")
            {
                NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Dashboard));
                nextActivityHandler.PassDataToNextActivity("username", Intent.GetStringExtra("username"));

                nextActivityHandler.NavigateToNextActivity(this);

            }

           

        }

        // Event handler for delete button
        public void DeleteBtn_Click(object sender, EventArgs e)
        {
            // Show dialog to confirm deletion
            DialogButtonClickHandler onOkayClick = () =>
            {
                // Delete task
                UserTask userTask = new UserTask(this);

                userTask.DeleteTask(username, tN, cC);
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

                return true;
            };
            // Cancel deletion
            DialogButtonClickHandler onCancelClick = () =>
            {
                return false;
            };

            // Show dialog
            custom_dialog dialog = new custom_dialog(this, "Are you sure you want to delete this task?", onOkayClick, onCancelClick);
            dialog.Show();
        }

        // Event handler for mark as done button
        public void MarkAsDoneBtn_Click(object sender, EventArgs e)
        {
            // Mark task as done
            UserTask userTask = new UserTask(this);
            userTask.TaskDone(username, tN, cC);

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
