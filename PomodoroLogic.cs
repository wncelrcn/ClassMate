using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace IT123P_FinalMP
{
    // class to handle pomodoro logic
    internal class PomodoroLogic
    {
        // class variables
        private Context context;
        private string minute, secs;
        private EditText minuteText, secsText;
        private Handler handler;
        private Action runnable;
        private bool isRunning;
        private Button startStopBtn;

        // constructor
        public PomodoroLogic(Context context, string minute, string secs, EditText minuteText, EditText secsText, Button startStopBtn)
        {
            this.context = context;
            this.minute = minute;
            this.secs = secs;
            this.minuteText = minuteText;
            this.secsText = secsText;
            this.handler = new Handler();
            this.isRunning = false;
            this.startStopBtn = startStopBtn;
        }

        // start pomodoro timer
        public void StartPomodoro()
        {
            // check if input is valid and not empty and not zero
            if (!int.TryParse(minute, out int min) || !int.TryParse(secs, out int sec) || (min == 0 && sec == 0))
            {
                Toast.MakeText(context, "Please input a valid time.", ToastLength.Short).Show();
                return;
            }
            // disable text fields
            minuteText.Enabled = false;
            secsText.Enabled = false;
            isRunning = true;

            // start timer runnable function
            runnable = () =>
            {
                // check if time is up
                if (min == 0 && sec == 0)
                {
                    handler.RemoveCallbacks(runnable);
                    Toast.MakeText(context, "Time's up!", ToastLength.Short).Show();
                    minuteText.Enabled = true;
                    startStopBtn.Text = "Start";
                    secsText.Enabled = true;
                    isRunning = false;
                }

                else
                {
                    // decrement time
                    if (isRunning == true)
                    {
                        if (sec == 0)
                        {
                            min--;
                            sec = 59;
                        }
                        else
                        {
                            sec--;
                        }
                        // update text fields
                        minuteText.Text = min.ToString("D2");
                        secsText.Text = sec.ToString("D2");
                        // call runnable function every second
                        handler.PostDelayed(runnable, 1000);
                    }
                    else
                    {
                        return;
                    }
                }
            };
            // call runnable function
            handler.Post(runnable);
        }
        // stop pomodoro timer
        public void StopPomodoro()
        {
            // stop timer
            if (runnable != null)
            {
                // remove callbacks
                handler.RemoveCallbacks(runnable);
            }
            // enable text fields
            minuteText.Enabled = true;
            secsText.Enabled = true;
            isRunning = false;
        }
    }
}
