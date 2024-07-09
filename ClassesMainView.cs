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
    [Activity(Label = "StudyApp", Theme = "@style/AppTheme", MainLauncher = false)]
    public class ClassesMainView : AppCompatActivity
    {
        TextView title, desc;
        BottomNavigationView bottomNavigationView;
        Button btnAddClass;
        LinearLayout classesLayoutContainer;
        string username;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.classes_main_layout);

            title = FindViewById<TextView>(Resource.Id.title);
            desc = FindViewById<TextView>(Resource.Id.desc);
            bottomNavigationView = FindViewById<BottomNavigationView>(Resource.Id.bottom_navigation_view);

            username = Intent.GetStringExtra("username");

            classesLayoutContainer = FindViewById<LinearLayout>(Resource.Id.classContainer);

            FontHandler boldFont = new FontHandler(this, "Raleway-Bold.ttf");
            FontHandler mediumFont = new FontHandler(this, "Raleway-Medium.ttf");
            FontHandler regularFont = new FontHandler(this, "Raleway-Regular.ttf");
            FontHandler semiBoldFont = new FontHandler(this, "Raleway-Semibold.ttf");

            semiBoldFont.SetFont(title);
            regularFont.SetFont(desc);
            

            btnAddClass = FindViewById<Button>(Resource.Id.addAClassBtn);
            btnAddClass.Click += AddClass;
            semiBoldFont.SetFont(btnAddClass);


            BottomNavigationViewLogic bottomNav = new BottomNavigationViewLogic(this, bottomNavigationView, username, "ClassesMainView");
            bottomNavigationView.SelectedItemId = Resource.Id.navigation_tasks;
            bottomNavigationView.NavigationItemSelected += bottomNav.BottomNavigationView_NavigationItemSelected;
            bottomNav.SetInitialSelectedItem("ClassesMainView");


            LoadClass();
        }

        public async void LoadClass()
        {
            UserClass userClass = new UserClass(this, classesLayoutContainer);
            await userClass.GetCurrStudClasses(username);
        }
      
        public void AddClass(object sender, EventArgs e)
        {
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