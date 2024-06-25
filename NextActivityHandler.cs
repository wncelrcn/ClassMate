using Android.App;
using Android.Content;
using Android.Widget;
using System;

namespace Mod3RESTTask
{
    internal class NextActivityHandler
    {

        private Context context;
        private string toastMsg;
        private Type currClass;
        Intent intent;

        public NextActivityHandler(Context context, string toastMsg, Type currClass)
        {
            this.context = context;
            this.toastMsg = toastMsg;
            this.currClass = currClass;
        }

        // Navigates to the next activity
        public void NavigateToNextActivity()
        {
            Toast.MakeText(context, toastMsg, ToastLength.Short).Show();

            intent = new Intent(context, currClass);
            context.StartActivity(intent);
        }
    }
}