using AzureOfflineSyncDemo.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AzureOfflineSyncDemo.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UserCreatePage : ContentPage
	{
		public UserCreatePage ()
		{
            //Set title of the page
            Title = "Create User";

            //Set Binding Context
            BindingContext = new UserCreateViewModel();

            //Initialize the UI Components
            InitializeComponent();
		}
	}
}