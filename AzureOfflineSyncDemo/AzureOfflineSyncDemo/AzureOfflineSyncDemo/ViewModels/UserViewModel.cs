using AzureOfflineSyncDemo.DI;
using AzureOfflineSyncDemo.Droid.DS;
using AzureOfflineSyncDemo.Models;
using AzureOfflineSyncDemo.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AzureOfflineSyncDemo.ViewModels
{
    class UserViewModel : BaseViewModel
    {

        public ObservableCollection<EmployeeItem> EmployeeItem { get; set; }

        public UserViewModel()
        {
            EmployeeItem = new ObservableCollection<EmployeeItem>();
        }
        AzureManager manager;

        /// <summary>
        /// Load user from database in list
        /// </summary>
        public async Task LoadUsersAsync()
        {
            EmployeeItem.Clear();
            manager = AzureManager.DefaultManager;
            EmployeeItem = await manager.GetUserAsync();
        }

        /// <summary>
        /// This command is used to Remove user from database as well as Azure server
        /// </summary>
        public async void OnDeletedCommandExecuted(object sender)
        {
            var obj = (sender as Button).BindingContext;
            var id = (obj as EmployeeItem).Id;
            var employee = EmployeeItem.FirstOrDefault(x => x.Id == id);
            
            if (employee == null)
            {
                await App.Current.MainPage.DisplayAlert("Error!", "User doesn't exist or has been deleted.", "Ok");
                return;
            }
            IsBusy = true;
            try
            {
                RemoveItemFromList(employee);
                await manager.Delete(employee);
                if (Xamarin.Forms.DependencyService.Get<IConnectivity>().IsFastInternet())
                {
                    manager.SyncAsync().NoAwait();
                }
            }
            catch (Exception ex)
            {
                await DisplayError(ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Remove user from list 
        /// </summary>

        private void RemoveItemFromList(EmployeeItem employee)
        {
            EmployeeItem.Remove(employee);
        }
    }
}
