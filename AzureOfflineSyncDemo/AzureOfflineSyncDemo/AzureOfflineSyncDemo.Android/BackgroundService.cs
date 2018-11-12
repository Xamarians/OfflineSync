using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using AzureOfflineSyncDemo.Droid.DS;
using AzureOfflineSyncDemo.Services;
using Plugin.Connectivity;

namespace AzureOfflineSyncDemo.Droid
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
                        SyncDatabase();
                }
            };
            return base.OnStartCommand(intent, flags, startId);
        }

        private async void SyncDatabase()
        {
            try
            {
                await AzureManager.DefaultManager.SyncAsync();
            }
            catch
            {
                // ignore for any error
            }
        }
    }
}