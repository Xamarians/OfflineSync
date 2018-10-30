using OfflineSyncDemo.Data;
using OfflineSyncDemo.Services;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace OfflineSyncDemo
{
    public partial class App : Application
    {
        public App()
        {
            //Initialize the UI Components
            InitializeComponent();

            //Initialize the Database
            Repository.InitializeDatabase();

            //Set the MainPage for the Application
            MainPage = new NavigationPage(new MainPage());

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
           SyncService.Instance.SyncAll();
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
