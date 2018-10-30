using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AzureOfflineSyncDemo.Controls
{
    class LoaderView : ContentView
    {
        private new View Content { get { return base.Content; } }

        public LoaderView()
            : this(new Binding("IsBusy"))
        {

        }

        /// <summary>
        /// Used to show loader 
        /// </summary>
        public LoaderView(BindingBase bindingBase)
        {
            HorizontalOptions = LayoutOptions.FillAndExpand;
            VerticalOptions = LayoutOptions.FillAndExpand;
            BackgroundColor = Color.FromHex("#03FFFFFF");
            var activityIndicator = new ActivityIndicator() { IsRunning = true, VerticalOptions = LayoutOptions.CenterAndExpand };
            base.Content = activityIndicator;
            SetBinding(IsVisibleProperty, bindingBase);
        }
    }
}
