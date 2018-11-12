using AzureOfflineSyncDemo.DI;
using AzureOfflineSyncDemo.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace AzureOfflineSyncDemo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var manager = AzureManager.DefaultManager;
            MainPage = new NavigationPage(new Views.MainPage());

            //Register Network Changed Event
            DependencyService.Get<DI.IDependencyService>().OnStatusChanged += App_OnStatusChanged;
        }

        /// <summary>
        /// This handler is used to sync the data to the database when app is connected with the Internet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void App_OnStatusChanged(object sender, EventArgs e)
        {
            if (DependencyService.Get<IConnectivity>().IsFastInternet())
                AzureManager.DefaultManager.SyncAsync().ContinueWith((t)=> { });
        }
        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
