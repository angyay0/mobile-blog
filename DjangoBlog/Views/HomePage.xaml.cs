using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DjangoBlog.Models.ResponseContracts;
using DjangoBlog.ViewModels;
using Xamarin.Forms;

namespace DjangoBlog.Views
{
    public partial class HomePage : ContentPage
    {
        ObservableCollection<Post> postList;

        public HomePage()
        {
            InitializeComponent();

            postList = new ObservableCollection<Post>();
            postList.Add(new Post
            {
                author = "Angel Perez",
                title = "Python and Xamarin Rules",
                created = DateTime.Now,
                edited = DateTime.Now,
                comments = new List<Comment>(),
                entry = "Python rules is awesome working as backend"
            });

            PostList.ItemsSource = postList;
        }

        void OnPostSelected(object sender,EventArgs e)
        {
            var element = (Post) PostList.SelectedItem;
            if (element != null){
                
                Navigation.PushAsync(new PostDetailPage(element));
                PostList.SelectedItem = null;
            }           
        }
    }
}
