using Android.Content;
using Android.OS;
using Android.Widget;
using System;

namespace IT123P_FinalMP
{
    internal class PomodoroLogic
    {
        private Context context;
        private string minute, secs;
        private EditText minuteText, secsText;
        private Handler handler;
        private Action runnable;
        private bool isRunning;
        private Button startStopBtn;

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

        public void StartPomodoro()
        {
            if (!int.TryParse(minute, out int min) || !int.TryParse(secs, out int sec) || (min == 0 && sec == 0))
            {
                Toast.MakeText(context, "Please input a valid time.", ToastLength.Short).Show();
                return;
            }
            
            minuteText.Enabled = false;
            secsText.Enabled = false;
            isRunning = true;

            runnable = () =>
            {
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

                        minuteText.Text = min.ToString("D2");
                        secsText.Text = sec.ToString("D2");

                        handler.PostDelayed(runnable, 1000);
                    }
                    else
                    {
                        return;
                    }
                }
            };

            handler.Post(runnable);
        }

        public void StopPomodoro()
        {
            if (runnable != null)
            {
                handler.RemoveCallbacks(runnable);
            }
            minuteText.Enabled = true;
            secsText.Enabled = true;
            isRunning = false;
        }
    }
}
