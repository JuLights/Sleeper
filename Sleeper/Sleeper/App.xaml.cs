using Sleeper.Views;
using Xamarin.Forms;

namespace Sleeper
{
    public partial class App : Application
    {
        public static string currentPage = "Chill";
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new GenrePage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
