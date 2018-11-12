using OfflineSyncDemo.Data;
using OfflineSyncDemo.DI;
using OfflineSyncDemo.Models;
using OfflineSyncDemo.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace OfflineSyncDemo.ViewModels
{
    class UserViewModel : BaseViewModel
    {
        public ObservableCollection<User> Users { get; set; }

        public UserViewModel()
        {
            Users = new ObservableCollection<User>();
            LoadUsersAsync();
        }

        /// <summary>
        /// Load user from database in list
        /// </summary>
        public void LoadUsersAsync()
        {
            Users.Clear();
            foreach (var item in Repository.AsQueryable<User>().Where(x => x.SyncStatus != (int)SyncStatus.Deleted))
            {
                Users.Add(item);
            }
        }
        /// <summary>
        /// Remove user from database 
        /// </summary>
        public async void OnDeletedCommandExecuted(object sender)
        {
            var obj = (sender as Button).BindingContext;
            var id = (obj as User).DbId;
            int userId = Convert.ToInt32(id);
            var user = Repository.FindOne<User>(userId);
            if (user == null)
            {
                await App.Current.MainPage.DisplayAlert("Error!", "User doesn't exist or has been deleted.", "Ok");
                RemoveItemFromList(userId);
                return;
            }
            IsBusy = true;
            try
            {

                Repository.SyncDelete(user);
                RemoveItemFromList(userId);
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
        /// Remove user from List 
        /// </summary>

        private void RemoveItemFromList(int id)
        {
            var u = Users.FirstOrDefault(x => x.DbId == id);
            if (u != null)
                Users.Remove(u);
        }


    }
}
