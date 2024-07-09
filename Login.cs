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

namespace IT123P_FinalMP
{
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Login : AppCompatActivity
    {
        Button loginButton;
        ImageButton returnBtn;
        EditText loginText, passwordText;
        TextView registerText, titleText, descText;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.login_layout);

            // Initialize Widgets Components
            loginButton = FindViewById<Button>(Resource.Id.loginBtn);
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            loginText = FindViewById<EditText>(Resource.Id.usernameTxt);
            passwordText = FindViewById<EditText>(Resource.Id.passwordTxt);
            registerText = FindViewById<TextView>(Resource.Id.registerTxt);
            descText = FindViewById<TextView>(Resource.Id.desc);

            // Set Title
            titleText = FindViewById<TextView>(Resource.Id.title);

            // Font Styling
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(titleText);
            regularFont.SetFont(descText);
            regularFont.SetFont(loginText);
            semiBoldFont.SetFont(registerText);
            regularFont.SetFont(passwordText);
            semiBoldFont.SetFont(loginButton);

            // Button Click Events
            loginButton.Click += LoginButton_Click;
            returnBtn.Click += ReturnBtn_Click;

            // Register Text Link Clickable
            string text = "Don't have an account? Sign up here!";
            SpannableString spannableString = new SpannableString(text);
            ClickableSpan clickableSpan = new CustomClickableSpan(this);
            spannableString.SetSpan(clickableSpan, 23, text.Length, SpanTypes.ExclusiveExclusive);
            registerText.TextFormatted = spannableString;
            registerText.MovementMethod = LinkMovementMethod.Instance;

            // Button Styling
            Styler.ApplyRoundedCorners(loginButton);
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
                activityHandler.NavigateToNextActivity(_context);
            }

            public override void UpdateDrawState(TextPaint ds)
            {
                base.UpdateDrawState(ds);
                ds.Color = Android.Graphics.Color.DarkGreen;  
                ds.UnderlineText = false; 
            }
        }

        // Login Button Click Event
        private void LoginButton_Click(object sender, System.EventArgs e)
        {
            string username = loginText.Text;
            string password = passwordText.Text;

            // Validation
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                Toast.MakeText(this, "Please fill up all fields.", ToastLength.Short).Show();
            }
            else
            {
                UserConnection userConnection = new UserConnection(this);
                userConnection.Login(username, password);
            }
        }

        // Return Button Click Event
        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Landing));
            nextActivityHandler.NavigateToNextActivity(this);
        }


        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
