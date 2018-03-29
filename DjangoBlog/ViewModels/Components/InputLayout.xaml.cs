using System;
using DjangoBlog.Helper;
using Xamarin.Forms;

namespace DjangoBlog.ViewModels.Components
{
    public partial class InputLayout : ContentView
    {
        Action<string> _callback;

        public InputLayout()
        {
            InitializeComponent();
            //Default Action
            _callback = (string s) =>
            {
                Constants.AppLogger("Empty Action");
            };
        }

        public void SetClickAction(Action<string> callback)
        {
            _callback = callback;
        }

        void OnSendClicked(object sender, EventArgs e)
        {
            string comment = CommentEntry.Text;
            if (!string.IsNullOrEmpty(comment))
            {
                _callback?.Invoke(comment);
                CommentEntry.Text = "";
            }
        }
    }
}
