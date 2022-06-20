using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sleeper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GenrePage : ContentPage
    {
        public GenrePage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            LogoutMessage().GetAwaiter();

            return true;
        }


        private async Task LogoutMessage()
        {
            var alertResult = await DisplayAlert("Sleeper", "Do you really want to exit?", "Yes", "No");
            if(alertResult)
                Process.GetCurrentProcess().Kill();
        }
    }
}