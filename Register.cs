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
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class Register : AppCompatActivity
    {

        Button returnBtn, nextBtn;
        TextView registerText, title, desc;
        EditText username, passwordTxt;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.register_layout);

            returnBtn = FindViewById<Button>(Resource.Id.returnBtn);
            registerText = FindViewById<TextView>(Resource.Id.registerTxt);
            title = FindViewById<TextView>(Resource.Id.title);
            desc = FindViewById<TextView>(Resource.Id.desc);
            nextBtn = FindViewById<Button>(Resource.Id.nextBtn);
            
            username = FindViewById<EditText>(Resource.Id.usernameTxt);
            passwordTxt = FindViewById<EditText>(Resource.Id.passwordTxt);

            returnBtn.Click += ReturnBtn_Click;
            nextBtn.Click += NextBtn_Click;

            string text = "Already have an account? Sign in. ";
            SpannableString spannableString = new SpannableString(text);

            ClickableSpan clickableSpan = new CustomClickableSpan(this);
            spannableString.SetSpan(clickableSpan, 25, text.Length, SpanTypes.ExclusiveExclusive);

            registerText.TextFormatted = spannableString;
            registerText.MovementMethod = LinkMovementMethod.Instance;

            ButtonStyler.ApplyRoundedCorners(nextBtn);

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
                activityHandler.NavigateToNextActivity(_context);
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
            nextActivityHandler.NavigateToNextActivity(this);
        }

        public void NextBtn_Click(object sender, System.EventArgs e)
        {
            string uname = username.Text;
            string pword = passwordTxt.Text;

            if (string.IsNullOrEmpty(uname) || string.IsNullOrEmpty(pword))
            {
                Toast.MakeText(this, "Please enter a username and password.", ToastLength.Short).Show();
                return;
            }

            else
            {

                UserConnection userConnection = new UserConnection(this);

                if (userConnection.CheckStudUname(username.Text))
                {

                    NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "Next...", typeof(Register_StudID));

                    nextActivityHandler.PassDataToNextActivity("username", username.Text);
                    nextActivityHandler.PassDataToNextActivity("password", passwordTxt.Text);
                    nextActivityHandler.NavigateToNextActivity(this);
                }
                else
                {
                    Toast.MakeText(this, "Please enter a new username.", ToastLength.Short).Show();
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
