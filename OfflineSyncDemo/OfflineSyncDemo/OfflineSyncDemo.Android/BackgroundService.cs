using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using OfflineSyncDemo.Services;
using Plugin.Connectivity;

namespace OfflineSyncDemo.Droid
{
    [Service]
    public class BackgroundService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            CrossConnectivity.Current.ConnectivityChanged += delegate
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    if (Connectivity.IsConnectedFast(Xamarin.Forms.Forms.Context))
                        SyncService.Instance.SyncAll();
                }
            };
            return base.OnStartCommand(intent, flags, startId);
        }

    }
}