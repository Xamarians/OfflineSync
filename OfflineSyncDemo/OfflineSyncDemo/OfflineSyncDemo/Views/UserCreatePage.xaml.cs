using OfflineSyncDemo.Models;
using OfflineSyncDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserCreatePage : ContentPage
    {        
        public UserCreatePage(User user)
        {
            //Set Title of the Page
            Title = "Create User";

            //Setting the Binding Context
            BindingContext = new UserCreateViewModel();

            //Intialize the UI Components.
            InitializeComponent();
        }

    }
}