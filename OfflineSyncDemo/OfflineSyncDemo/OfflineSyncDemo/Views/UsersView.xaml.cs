using OfflineSyncDemo.Models;
using OfflineSyncDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UsersView : ContentPage
	{
        UserViewModel viewModel;
		public UsersView ()
		{
            //Set Title of the Page
            Title = "Users";

            //Setting the Binding Context
            BindingContext = viewModel = new UserViewModel();

            //Intialize the UI Components.
            InitializeComponent();
		}

        /// <summary>
        /// This handler is used to select the tapped user and display the details onto UserUpdatePage.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LstUsersItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem;
            if (item == null)
                return;
            (sender as ListView).SelectedItem = null;
            Navigation.PushAsync(new UserUpdatePage((User)item));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.LoadUsersAsync();
        }

        /// <summary>
        /// This handler is used to initiate the command in viewModel to delete the selected User.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteButtonClicked(object sender, System.EventArgs e)
        {
            viewModel.OnDeletedCommandExecuted(sender);           
        }
    }
}