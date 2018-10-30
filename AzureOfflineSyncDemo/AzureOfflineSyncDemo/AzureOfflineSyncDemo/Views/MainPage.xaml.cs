using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzureOfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPage
	{
		public MainPage ()
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
        private void ButtonAddUserClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserCreatePage());
        }

        /// <summary>
        /// This handler is used to navigate on UsersViewPage where all the users are listed. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonEditUserClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserViewPage());
        }
    }
}