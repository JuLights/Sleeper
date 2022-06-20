using MediaManager;
using Sleeper.Models;
using Sleeper.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Sleeper.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SleeperMainPage : ContentPage
    {

        public SleeperMainPage()
        {
            InitializeComponent();

            //CrossMediaManager.Current.Init();
        }

        public SleeperMainPage(MusicGenre musicGenre)
        {
            InitializeComponent();

            BindingContext = new SleeperMainPageViewModel(musicGenre);
        }

        private async void Slider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            TimeSpan currentMediaPosition = CrossMediaManager.Current.Position;
            TimeSpan currentMediaDuration = CrossMediaManager.Current.Duration;
            TimeSpan ts = new TimeSpan((long)e.NewValue);
            if(currentMediaPosition.Ticks< (long)e.NewValue + 5000)
            {
                await CrossMediaManager.Current.SeekTo(ts);
            }else if((long)e.NewValue < (long)e.OldValue)
            {
                await CrossMediaManager.Current.SeekTo(ts);
            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CrossMediaManager.Current.Init();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            var player = CrossMediaManager.Current.MediaPlayer;
            player.Stop();
            CrossMediaManager.Current.Dispose();
        }


    }
}