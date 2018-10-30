using AzureOfflineSyncDemo.Models;
using AzureOfflineSyncDemo.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzureOfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserViewPage : ContentPage
    {
        UserViewModel viewModel;

        public UserViewPage()
        {
            //Set Title of the page.
            Title = "Users";
           
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
            Navigation.PushAsync(new UserUpdatePage((EmployeeItem)item));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            viewModel = new UserViewModel();
            await viewModel.LoadUsersAsync();

            //Set Binding Context
            BindingContext = viewModel;

            //Initialize the UI Components
            InitializeComponent();
        }

        /// <summary>
        /// This handler is used to initiate the command in viewModel to delete the selected User.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDeleteButtonClicked(object sender, EventArgs e)
        {
            viewModel.OnDeletedCommandExecuted(sender);
        }
    }
}