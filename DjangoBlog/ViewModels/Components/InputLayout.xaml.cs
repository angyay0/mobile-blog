using System;
using System.Collections.Generic;
using DjangoBlog.Helper;
using Xamarin.Forms;

namespace DjangoBlog.ViewModels.Components
{
    public partial class InputLayout : ContentView
    {
        Action _callback;

        public InputLayout()
        {
            InitializeComponent();
            //Default Action
            _callback = () =>
            {
                Constants.AppLogger("Empty Action");
            };
        }

        public void SetClickAction(Action callback)
        {
            _callback = callback;
        }

        void OnSendClicked(object sender, EventArgs e)
        {
            _callback?.Invoke();
        }
    }
}
