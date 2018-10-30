using OfflineSyncDemo.Data;
using OfflineSyncDemo.Models;
using OfflineSyncDemo.Views;
using System;
using System.Linq;
using Xamarin.Forms;

namespace OfflineSyncDemo
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            //Initialize the UI Components
            InitializeComponent();

            //Hide the Navigation bar for this Page only.
            NavigationPage.SetHasNavigationBar(this, false);
        }

        /// <summary>
        /// This handler is used to navigate on the UserCreatePage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAddUserClicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new UserCreatePage(new User()));
        }

        /// <summary>
        /// This handler is used to navigate on UsersView Page where all the users are listed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEditUserClicked(object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new UsersView());
        }
    }
}
