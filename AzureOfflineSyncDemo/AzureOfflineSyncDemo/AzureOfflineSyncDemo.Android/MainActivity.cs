using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AzureOfflineSyncDemo.DI;
using AzureOfflineSyncDemo.Services;

namespace AzureOfflineSyncDemo.Droid
{
    [Activity(Label = "AzureOfflineSyncDemo", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (Xamarin.Forms.DependencyService.Get<IConnectivity>().IsFastInternet())
                AzureManager.DefaultManager.SyncAsync().ContinueWith((t) => { });

            //Check for service. 
            var mServiceIntent = new Intent(this, typeof(BackgroundService));
            StartService(mServiceIntent);

            //if (!IsMyServiceRunning(Java.Lang.Class.FromType(typeof(BackgroundService))))
            //{
            //    StartService(mServiceIntent);
            //}
        }


        //public bool IsMyServiceRunning(Java.Lang.Class serviceClass)
        //{
        //    ActivityManager manager = (ActivityManager)GetSystemService(ActivityService);
        //    foreach (var item in manager.GetRunningServices(int.MaxValue))
        //    {
        //        if (serviceClass.Name.Equals(item.Service.ClassName))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
    }
}