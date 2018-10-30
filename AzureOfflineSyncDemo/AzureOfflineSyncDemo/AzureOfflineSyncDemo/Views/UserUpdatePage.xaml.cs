using AzureOfflineSyncDemo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzureOfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserUpdatePage : ContentPage
    {
        public UserUpdatePage(EmployeeItem user)
        {
            //Set Title of the page
            Title = "Update User";

            //Set Binding Context
            BindingContext = new ViewModels.UserCreateViewModel(user);

            //Initialize the UI Components
            InitializeComponent();
        }
    }
}