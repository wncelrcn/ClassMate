using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace IT123P_FinalMP
{
    public delegate bool DialogButtonClickHandler();
    // Class for custom dialog pop up
    public class custom_dialog : Dialog
    {
        
        private Context context;
        private string text;
        private DialogButtonClickHandler onOkayClick;
        private DialogButtonClickHandler onCancelClick;

        public custom_dialog(Context context, string text, DialogButtonClickHandler onOkayClick, DialogButtonClickHandler onCancelClick) : base(context)
        {
            this.context = context;
            this.text = text;
            this.onOkayClick = onOkayClick;
            this.onCancelClick = onCancelClick;
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Inflate the custom layout
            LayoutInflater inflater = LayoutInflater.From(context);
            View dialogView = inflater.Inflate(Resource.Layout.custom_dialog, null);
            SetContentView(dialogView);

            // Get references to the views in the custom layout
            TextView textContainer = dialogView.FindViewById<TextView>(Resource.Id.textContainer);
            textContainer.Text = text;

            Button btnOkay = dialogView.FindViewById<Button>(Resource.Id.btnOkay);
            Button btnCancel = dialogView.FindViewById<Button>(Resource.Id.btnCancel);

            // Set button click events
            btnOkay.Click += (s, args) =>
            {
                bool result = onOkayClick.Invoke();
            };

            btnCancel.Click += (s, args) =>
            {
                bool result = onCancelClick.Invoke();
                Dismiss();
            };
        }

    }
}
