using OfflineSyncDemo.Models;
using System.Threading.Tasks;

namespace OfflineSyncDemo.ViewModels
{
    public abstract class BaseViewModel : NotifyPropertyChangedBase
    {
        bool _isbusy;
        public bool IsBusy
        {
            get { return _isbusy; }
            set
            {
                SetProperty(ref _isbusy, value);
            }
        }

        protected Task DisplayError(string message)
        {
            return App.Current.MainPage.DisplayAlert("Error!", message, "Ok");
        }
    }

}
