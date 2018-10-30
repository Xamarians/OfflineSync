
using OfflineSyncDemo.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserUpdatePage : ContentPage
	{
		public UserUpdatePage (User user)
		{
            //Set Title of the Page
            Title = "Update User";

            //Set the Binding Context
            BindingContext = new ViewModels.UserCreateViewModel(user);

            //Initialize the UI Components
            InitializeComponent();
        }
	}
}