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
            string email = EmailEntry.Text;
            string name = NameEntry.Text;
            string last = LastEntry.Text;
            string pass = PassEntry.Text;

            //Just Simply Validate that arent empty strings
            if (!string.IsNullOrEmpty(name))
            {
                if (!string.IsNullOrEmpty(last))
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        if (!string.IsNullOrEmpty(pass))
                        {
                            IsLoading = true;
                            SignupRequest(new SignupRequest
                            {
                                name = name,
                                last = last,
                                email = email,
                                password = pass
                            });
                        }
                        else
                        {
                            DisplayAlert("Information", "Password cannot be empty", "Ok");
                        }
                    }
                    else
                    {
                        DisplayAlert("Information", "Email cannot be empty", "Ok");
                    }
                }
                else
                {
                    DisplayAlert("Information", "Last Name cannot be empty", "Ok");
                }
            }
            else
            {
                DisplayAlert("Information", "Name cannot be empty", "Ok");
            }
        }

        void OnCancelClicked(object sender, EventArgs eventArgs)
        {
            Navigation.PopAsync();
        }

        async Task SignupRequest(SignupRequest request)
        {
            var datasource = DjangoAPI.Instance();
            var result = await datasource.SignupRequest(request);
            Device.BeginInvokeOnMainThread(() =>
            {
                IsLoading = false;
                if (result != null && result.code == 0)
                {
                    DisplayAlert("Information", "Signup proccess completed. Please login with your credentials", "Ok");
                    Navigation.PopAsync();
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
