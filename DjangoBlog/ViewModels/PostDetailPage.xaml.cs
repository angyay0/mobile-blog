using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DjangoBlog.Models.ResponseContracts;
using Xamarin.Forms;

namespace DjangoBlog.ViewModels
{
    public partial class PostDetailPage : ContentPage
    {
        Post post;
        ObservableCollection<Comment> commentList;

        public PostDetailPage(Post item)
        {
            InitializeComponent();
            post = item;

            Populate();
        }

        void Populate()
        {
            PostAuthor.Text = post.author;
            Entry.Text = post.entry;
            Title = post.title;

            commentList = new ObservableCollection<Comment>(post.comments);
            CommentsList.ItemsSource = commentList;
        }
    }
}
