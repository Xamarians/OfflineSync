using AzureOfflineSyncDemo.Models;
using AzureOfflineSyncDemo.Services;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace AzureOfflineSyncDemo.ViewModels
{
    public class UserCreateViewModel : BaseViewModel
    {
        private readonly string UserId;
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }

        AzureManager manager;
        #region Bindable Properties

        string _firstname;
        public string FirstName
        {
            get
            {
                return _firstname;
            }
            set
            {
                SetProperty(ref _firstname, value);
            }
        }

        string _lastName;
        public string LastName
        {
            get
            {
                return _lastName;
            }
            set
            {
                SetProperty(ref _lastName, value);
            }
        }

        string _email;
        public string Email
        {
            get
            {
                return _email;
            }
            set
            {
                SetProperty(ref _email, value);
            }
        }
        string _phoneNumber;
        public string PhoneNumber
        {
            get
            {
                return _phoneNumber;
            }
            set
            {
                SetProperty(ref _phoneNumber, value);
            }
        }

        string _address;
        public string Address
        {
            get
            {
                return _address;
            }
            set
            {
                SetProperty(ref _address, value);
            }
        }

        #endregion

        public UserCreateViewModel()
        {
            CreateCommand = new Command(OnCreateCommandExecuted);
            UpdateCommand = new Command(OnUpdateCommandExecuted);
            manager = AzureManager.DefaultManager;
        }

        public UserCreateViewModel(EmployeeItem user) :
            this()
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.Phone;
            Address = user.Address;
        }

        /// <summary>
        /// This command is used for create a new user in Local db as well as on the Azure.
        /// </summary>
        private async void OnCreateCommandExecuted(object obj)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                await DisplayError("Firstname is required");
                return;
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                await DisplayError("Email is required");
                return;
            }
            if (IsBusy)
                return;
            var user = new EmployeeItem()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Address = Address,
                Phone = PhoneNumber,
            };

            IsBusy = true;
            try
            {
                await manager.SaveTaskAsync(user);
                manager.SyncAsync();

                await App.Current.MainPage.Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayError(ex.ToString());
            }
            finally
            {
                IsBusy = false;
            }
        }
        /// <summary>
        /// This command is used for Update the user in Local db as well as on the Azure server.
        /// </summary>
        private async void OnUpdateCommandExecuted(object obj)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                await DisplayError("Firstname is required");
                return;
            }
            if (IsBusy)
                return;
            var user = new EmployeeItem()
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Address = Address,
                Phone = PhoneNumber,
                Id = UserId
            };

            IsBusy = true;
            try
            {
                await manager.SaveTaskAsync(user);
                manager.SyncAsync().NoAwait();

                await App.Current.MainPage.Navigation.PopAsync();
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
    }
}
