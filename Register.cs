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
using Mod3RESTTask;

namespace IT123P_FinalMP
{
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register : AppCompatActivity
    {

        Button returnBtn, nextBtn;
        TextView registerText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.register_layout);

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            registerText = FindViewById<TextView>(Resource.Id.registerTxt);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);

            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;

            string text = "Already have an account? Sign in. ";
            SpannableString spannableString = new SpannableString(text);

            ClickableSpan clickableSpan = new CustomClickableSpan(this);
            spannableString.SetSpan(clickableSpan, 25, text.Length, SpanTypes.ExclusiveExclusive);

            registerText.TextFormatted = spannableString;
            registerText.MovementMethod = LinkMovementMethod.Instance;

        }

        private void NextBtn_Click1(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private class CustomClickableSpan : ClickableSpan
        {
            private readonly Context _context;

            public CustomClickableSpan(Context context)
            {
                _context = context;
            }

            public override void OnClick(View widget)
            {
                NextActivityHandler activityHandler = new NextActivityHandler(_context, "", typeof(Login));
                activityHandler.NavigateToNextActivity();
            }

            public override void UpdateDrawState(TextPaint ds)
            {
                base.UpdateDrawState(ds);
                ds.Color = Android.Graphics.Color.Blue; // Change text color
                ds.UnderlineText = false; // Remove underline
            }
        }

        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Landing));
            nextActivityHandler.NavigateToNextActivity();
        }

        public void NextBtn_Click(object sender, System.EventArgs e)
        {
           NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_StudID));
           nextActivityHandler.NavigateToNextActivity();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
