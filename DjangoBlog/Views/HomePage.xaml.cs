using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DjangoBlog.Helper;
using DjangoBlog.Models.ResponseContracts;
using DjangoBlog.ViewModels;
using Xamarin.Forms;

namespace DjangoBlog.Views
{
    public partial class HomePage : ContentPage, INotifyPropertyChanged
    {
        ObservableCollection<Post> postList;

        public HomePage()
        {
            InitializeComponent();
            InitializeBinding();

            FetchPosts();
        }

        void OnPostSelected(object sender,EventArgs e)
        {
            var element = (Post) PostList.SelectedItem;
            if (element != null){
                
                Navigation.PushAsync(new PostDetailPage(element));
                PostList.SelectedItem = null;
            }           
        }

        async Task FetchPosts()
        {
            var datasource = DjangoAPI.Instance();
            var res = await datasource.FetchPosts();
            Device.BeginInvokeOnMainThread(() =>
            {
                if (res != null && res.Count > 0)
                {
                    postList = new ObservableCollection<Post>(res);
                    PostList.ItemsSource = postList;
                }
                else
                {
                    DisplayAlert("Information", "Data could not be obtained, try again", "Ok");
                }
            });
        }

        #region Activity Indicator Bindings
        /// <summary>
        /// Initializes the binding Content for Activity Indicator.
        /// </summary>
        void InitializeBinding()
        {
            IsRefreshing = false;
            BindingContext = this;
        }
        //Pull To Refresh 
        private bool isRefreshig;
        public bool IsRefreshing
        {
            get
            {
                return this.isRefreshig;
            }

            set
            {
                this.isRefreshig = value;
                RaisePropertyChanged("IsRefreshing");
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

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await FetchPosts();
                    IsRefreshing = false;
                });
            }
        }

        public ICommand AddClickedCommand
        {
            get
            {
                return new Command(() =>
                {
                    Navigation.PushAsync(new CreatePostPage());
                });
            }
        }
        #endregion
    }
}
