using OfflineSyncDemo.Data;
using OfflineSyncDemo.Models;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace OfflineSyncDemo.ViewModels
{
    public class UserCreateViewModel : BaseViewModel
    {
        private readonly int UserId;
        public ICommand CreateCommand { get; private set; }
        public ICommand UpdateCommand { get; private set; }


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
        }

        public UserCreateViewModel(User user) :
            this()
        {
            UserId = user.DbId;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            PhoneNumber = user.Phone;
            Address = user.Address;
        }

        /// <summary>
        /// This command is used for create a new user in Local db as well as on the server.
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
            var user = new User()
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
                var dbUser = Repository.FindOne<User>(x => x.Email == user.Email);
                if (dbUser != null)
                {
                    await App.Current.MainPage.DisplayAlert("Error", "User already exist in the database.", "ok");
                    return;
                }
                await Repository.SyncInsert(user);
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


        /// <summary>
        /// This command is used for Update the user in Local db as well as on the server.
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
            var user = Repository.FindOne<User>(UserId);
            if (user == null)
            {
                await App.Current.MainPage.DisplayAlert("Error!", "User doesn't exist or has been deleted.", "Ok");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Address = Address;
            user.Phone = PhoneNumber;
            IsBusy = true;
            try
            {
                await Repository.SyncUpdate(user);
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
