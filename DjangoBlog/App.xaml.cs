using DjangoBlog.Views;
using Xamarin.Forms;

namespace DjangoBlog
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }
    }
}
