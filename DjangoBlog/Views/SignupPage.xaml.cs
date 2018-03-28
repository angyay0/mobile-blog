using System;
using System.ComponentModel;

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
