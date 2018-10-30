using System;
using OfflineSyncDemo.DI;
using OfflineSyncDemo.Droid.DS;
using Plugin.Connectivity;

[assembly: Xamarin.Forms.Dependency(typeof(DependencyService))]

namespace OfflineSyncDemo.Droid.DS
{
    public class DependencyService : IDependencyService
    {
        public event EventHandler OnStatusChanged;
        public DependencyService()
        {           
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                if (CrossConnectivity.Current.IsConnected)
                {
                    OnStatusChanged?.Invoke(null, EventArgs.Empty);
                }
            };
        }       
    }
}