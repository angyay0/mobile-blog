using System;
using System.ComponentModel;
using System.Threading.Tasks;
using DjangoBlog.Helper;
using DjangoBlog.Models.RequestContracts;
using Xamarin.Forms;

namespace DjangoBlog.Views
{
    public partial class LoginPage : ContentPage, INotifyPropertyChanged
    {
        public LoginPage()
        {
            InitializeComponent();
            InitializeBinding();
        }

        void OnLoginClicked(object sender, EventArgs e)
        {
            string user = UserEntry.Text;
            string pass = PassEntry.Text;

            if (!string.IsNullOrEmpty(user))
            {
                if (!string.IsNullOrEmpty(pass))
                {
                    IsLoading = true;
                    AuthenticateAttempt(new AuthRequest
                    {
                        user = user,
                        password = pass,
                        dev_id = 0,
                        dev_desc = "Xamarin.Forms "+ Device.RuntimePlatform
                    });
                }
                else
                {
                    DisplayAlert("Information", "Password Cannot be empty", "Ok");
                }
            }
            else
            {
                DisplayAlert("Information", "Email Cannot be empty", "Ok");
            }
        }

        void OnSignupClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SignupPage());
        }

        async Task AuthenticateAttempt(AuthRequest request)
        {
            var datasource = DjangoAPI.Instance();
            var result = await datasource.Authenticate(request);
            Device.BeginInvokeOnMainThread(() =>
            {
                IsLoading = false;
                if (result != null && result.code == 0)
                {
                    Application.Current.MainPage = new NavigationPage(new HomePage());
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
