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


namespace IT123P_FinalMP
{
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register : AppCompatActivity
    {
        // widget declarations
        Button nextBtn;
        ImageButton returnBtn;
        TextView registerText, title, desc;
        EditText username, passwordTxt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.register_layout);

            // widget initialization
            returnBtn = FindViewById<ImageButton>(Resource.Id.returnBtn);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);
            registerText = FindViewById<TextView>(Resource.Id.registerTxt);
            title = FindViewById<TextView>(Resource.Id.title);
            desc = FindViewById<TextView>(Resource.Id.desc);
            username = FindViewById<EditText>(Resource.Id.usernameTxt);
            passwordTxt = FindViewById<EditText>(Resource.Id.passwordTxt);

            // Button Click Events
            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;

            // Login Text Link Clickable
            string text = "Already have an account? Sign in. ";
            SpannableString spannableString = new SpannableString(text);
            ClickableSpan clickableSpan = new CustomClickableSpan(this);
            spannableString.SetSpan(clickableSpan, 25, text.Length, SpanTypes.ExclusiveExclusive);
            registerText.TextFormatted = spannableString;
            registerText.MovementMethod = LinkMovementMethod.Instance;

            // Button Styling
            Styler.ApplyRoundedCorners(nextBtn);

            // Font Styling
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            boldFont.SetFont(title);
            regularFont.SetFont(desc);
            regularFont.SetFont(username);
            regularFont.SetFont(passwordTxt);
            semiBoldFont.SetFont(nextBtn);
            semiBoldFont.SetFont(registerText);
        }

        // Custom Clickable Span Class
        private class CustomClickableSpan : ClickableSpan
        {
            private readonly Context _context;
            // Constructor
            public CustomClickableSpan(Context context)
            {
                _context = context;
            }

            // Click Event
            public override void OnClick(View widget)
            {
                NextActivityHandler activityHandler = new NextActivityHandler(_context, "", typeof(Login));
                activityHandler.NavigateToNextActivity(_context);
            }
            // Update Draw State
            public override void UpdateDrawState(TextPaint ds)
            {
                // Call Base Method
                base.UpdateDrawState(ds);
                ds.Color = Android.Graphics.Color.DarkGreen; 
                ds.UnderlineText = false;
            }
        }

        // Return Button Click Event
        public void ReturnBtn_Click(object sender, System.EventArgs e)
        {
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Returning...", typeof(Landing));
            nextActivityHandler.NavigateToNextActivity(this);
        }

        // Next Button (Dashboard) Click Event
        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            string uname = username.Text;
            string pword = passwordTxt.Text;

            // Validation
            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pword))
            {
                Toast.MakeText(this, "Please enter a username and password.", ToastLength.Short).Show();
                return;
            }
            else
            {
                // Register Logic
                UserConnection userConnection = new UserConnection(this);

                if (userConnection.CheckStudUname(username.Text))
                {
                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(Register_StudID));

                    nextActivityHandler.PassDataToNextActivity("username", username.Text);
                    nextActivityHandler.PassDataToNextActivity("password", passwordTxt.Text);
                    nextActivityHandler.NavigateToNextActivity(this);
                }

                // Username not available
                else
                {
                    Toast.MakeText(this, "Username not available. Please enter a new username.", ToastLength.Short).Show();
                    return;
                }

            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
