using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Android.Views;
using Android.Content;
using Google.Android.Material.BottomNavigation;
using System;
using System.Threading.Tasks;

namespace IT123P_FinalMP
{
    [Activity(Label = "ClassMate", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClassesMainView : AppCompatActivity
    {

        // widget declarations
        TextView title, desc;
        BottomNavigationView bottomNavigationView;
        Button btnAddClass;
        LinearLayout classesLayoutContainer;
        string username;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            
            SetContentView(Resource.Layout.classes_main_layout);

            // widget initialization
            title = FindViewById<TextView>(Resource.Id.title);
            desc = FindViewById<TextView>(Resource.Id.desc);
            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);
            classesLayoutContainer = FindViewById<LinearLayout>(Resource.Id.classContainer);
            btnAddClass = FindViewById<Button>(Resource.Id.addAClassBtn);

            // get the username from the intent
            username = Intent.GetStringExtra("username");

            // set the font of the widgets
            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(title);
            regularFont.SetFont(desc);
            semiBoldFont.SetFont(btnAddClass);

            // event handler for the add class button
            btnAddClass.Click += AddClass;

            // bottom navigation view logic
            BottomNavigationViewLogic bottomNav = new BottomNavigationViewLogic(this, bottomNavigationView, username, "ClassesMainView");
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_tasks;
            bottomNavigationView.NavigationItemSelected += bottomNav.BottomNavigationView_NavigationItemSelected;
            bottomNav.SetInitialSelectedItem("ClassesMainView");

            // load the classes
            LoadClass();
        }

        // method to load the classes
        public async void LoadClass()
        {
            UserClass userClass = new UserClass(this, classesLayoutContainer);
            await userClass.GetCurrStudClasses(username);
        }
        
        // event handler for the add class button
        public void AddClass(object sender, EventArgs e)
        {
            // navigate to the add class view
            NextActivityHandler nextActivityHandler = new NextActivityHandler(this, "", typeof(ClassesAddView));
            nextActivityHandler.PassDataToNextActivity("username", username);

            nextActivityHandler.NavigateToNextActivity(this);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}