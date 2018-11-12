using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;

namespace OfflineSyncDemo.Droid
{
    [Activity(Label = "OfflineSyncDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
            
            
            base.OnCreate(savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        protected override void OnResume()
        {
            base.OnResume();
            Services.SyncService.Instance.SyncAll();

            
            //Check for service.
            var mServiceIntent = new Intent(this, typeof(BackgroundService));
            if (!IsMyServiceRunning(Java.Lang.Class.FromType(typeof(BackgroundService))))
            {
                StartService(mServiceIntent);
            }
        }

      

        public bool IsMyServiceRunning(Java.Lang.Class serviceClass)
        {
            ActivityManager manager = (ActivityManager)GetSystemService(Context.ActivityService);
            foreach (var item in manager.GetRunningServices(int.MaxValue))
            {
                if (serviceClass.Name.Equals(item.Service.ClassName))
                {
                    return true;
                }
            }
            return false;
        }
    }
}