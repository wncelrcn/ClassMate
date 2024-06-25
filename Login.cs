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
using Android.Graphics;

namespace IT123P_FinalMP
{
    [Activity(Label = "Study Companion App", Theme = "@style/AppTheme", MainLauncher = true)]
    public class Login : AppCompatActivity
    {
        Button loginButton;
        EditText loginText, passwordText;
        TextView registerText, titleText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.login_layout);

            loginButton = FindViewById<Button>(Resource.Id.loginBtn);
            loginText = FindViewById<EditText>(Resource.Id.usernameTxt);
            passwordText = FindViewById<EditText>(Resource.Id.passwordTxt);
            registerText = FindViewById<TextView>(Resource.Id.registerTxt);

            // title
            titleText = FindViewById<TextView>(Resource.Id.title);

            FontHandler fontHandler = new FontHandler(this, "Raleway-Bold.ttf");
            fontHandler.SetFont(titleText);

            //loginButton.Click += LoginButton_Click;

            string text = "Don't have an account? Sign up here!";
            SpannableString spannableString = new SpannableString(text);

            ClickableSpan clickableSpan = new CustomClickableSpan(this);
            spannableString.SetSpan(clickableSpan, 23, text.Length, SpanTypes.ExclusiveExclusive);

            registerText.TextFormatted = spannableString;
            registerText.MovementMethod = LinkMovementMethod.Instance;



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
                NextActivityHandler activityHandler = new NextActivityHandler(_context, "register layout", typeof(Register));
                activityHandler.NavigateToNextActivity();
            }

            public override void UpdateDrawState(TextPaint ds)
            {
                base.UpdateDrawState(ds);
                ds.Color = Android.Graphics.Color.Blue; // Change text color
                ds.UnderlineText = false; // Remove underline
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
