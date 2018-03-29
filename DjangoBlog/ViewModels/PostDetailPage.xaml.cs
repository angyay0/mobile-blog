using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DjangoBlog.Helper;
using DjangoBlog.Models.RequestContracts;
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

            InputLayout.SetClickAction(CreateComment);
        }

        void CreateComment(string comment)
        {
            SendCommentToServer(comment);
        }

        async Task SendCommentToServer(string comment)
        {
            var datasource = DjangoAPI.Instance();
            var result = await datasource.CreateAComment(new CommentRequest
            {
                post = post.id,
                comment = comment
            });
            Device.BeginInvokeOnMainThread(() =>
            {
                if (result == 0)
                {
                    commentList.Add(new Comment
                    {
                        author = Constants.user.name + " " + Constants.user.last,
                        comment = comment
                    });
                }
                else
                {
                    DisplayAlert("Information", "An error occurs", "Ok");
                }
            });
        }
    }
}
