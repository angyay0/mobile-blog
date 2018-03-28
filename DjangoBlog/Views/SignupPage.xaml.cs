using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DjangoBlog.Helper;
using DjangoBlog.Models.RequestContracts;
using Xamarin.Forms;

namespace DjangoBlog.Views
{
    public partial class SignupPage : ContentPage, INotifyPropertyChanged
    {
        public SignupPage()
        {
            InitializeComponent();
        }

        void OnSignupClicked(object sender, EventArgs eventArgs)
        {
            
        }

        void OnCancelClicked(object sender, EventArgs eventArgs)
        {
            Navigation.PopAsync();
        }

        async Task SignupRequest(SignupRequest request)
        {
            IsLoading = true;
            var datasource = DjangoAPI.Instance();
            var result = await datasource.SignupRequest(request);
            Device.BeginInvokeOnMainThread(() =>
            {
                IsLoading = false;
                if (result != null && result.code == 0)
                {
                    DisplayAlert("Information", "Signup proccess completed. Please login with your credentials", "Ok");
                }
                else
                {
                    DisplayAlert("Information", "An Error occurs, please try again", "Ok");
                }
            });
        }

        #region Activity Indicator Binding
        void InitializeBinding()
        {
            IsLoading = false;
            BindingContext = this;
        }

        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
      
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }

        }
        #endregion
    }
}
