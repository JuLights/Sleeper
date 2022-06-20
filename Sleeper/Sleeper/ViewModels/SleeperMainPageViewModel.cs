using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediaManager;
using Sleeper.Models;
using Xamarin.Forms;

namespace Sleeper.ViewModels
{
    //[INotifyPropertyChanged]
    public partial class SleeperMainPageViewModel : ObservableObject
    {

        [ObservableProperty]
        List<Music> musicList;

        [ObservableProperty]
        string duration;

        [ObservableProperty]
        string elapsedTime;

        [ObservableProperty]
        long sliderValue;

        [ObservableProperty]
        long sliderMax;

        [ObservableProperty]
        string genre = "";

        public static bool isPlaying = false;

        Music previouslySelected;
        Music selectedMusic;
        public Music SelectedMusic
        {
            get => selectedMusic;
            set
            {
                if(value != null)
                {
                    //Application.Current.MainPage.DisplayAlert("selected", value.Name, "ok");
                    previouslySelected = value;
                    Play(previouslySelected.Path).GetAwaiter();
                    value = null;
                }

                selectedMusic = value;
                OnPropertyChanged();
            }
        }

        MediaManager.Library.IMediaItem player;

        public SleeperMainPageViewModel()
        {
            //var currentpage = AppShell.Current.CurrentPage.Title;
            //var kk = currentpage;



            musicList = new List<Music>()
            {
                new Music { Id = 0, Author = "Royal Godwin", Name = "What you gonn' DoA", Path = "RoyalGodwinWhatYouGonnDoA.mp3"},
                new Music { Id = 1, Author = "Thom Yorke", Name = "5.17", Path = "ThomYorke517.mp3"}
            };

            CrossMediaManager.Current.Init();

            ElapsedTime = "00:00";
            Duration = "00:00:00";

            SliderValue = 0;

            SliderMax = 1500;

        }

        
        public SleeperMainPageViewModel(MusicGenre musicGenre)
        {
            //events?

            Genre ="Music Genre: "+ musicGenre.Genre;

            if (musicGenre.Genre.ToLower() == "chill")
            {
                musicList = new List<Music>()
                {
                    //new Music { Id = 0, Author = "Royal Godwin", Name = "What you gonn' DoA", Path = "RoyalGodwinWhatYouGonnDoA.mp3"},
                    //new Music { Id = 1, Author = "Thom Yorke", Name = "5.17", Path = "ThomYorke517.mp3"}
                    new Music { Id = 0, Author = "BalanceBay", Name = "ambient-piano", Path = "ambient-piano-prod-by-balancebay-109400.mp3"},
                    new Music { Id = 1, Author = "AlanFrijns", Name = "falkirk-meditative-ambient-soundscape-for-learning-and-relaxing", Path = "falkirk-meditative-ambient-soundscape-for-learning-and-relaxing-95404.mp3"},
                    new Music { Id = 2, Author = "BluBonRelaXon", Name = "fall-asleep-like-a-baby", Path = "fall-asleep-like-a-baby-relax-music-blubon-relaxon-9643.mp3"}
                    //new Music { Id = 3, Author = "Thom Yorke", Name = "5.17", Path = "ThomYorke517.mp3"}
                };
            }
            else if(musicGenre.Genre.ToLower() == "ambient")
            {
                musicList = new List<Music>()
                {
                    new Music { Id = 0, Author = "ZakharValaha", Name = "ambiant-relax-sounds", Path = "ambiant-relax-sounds-10621.mp3"},
                    new Music { Id = 1, Author = "NaturesEye", Name = "ancient-frequencies-atonal-meditation", Path = "ancient-frequencies-atonal-meditation-112529.mp3"},
                    new Music { Id = 2, Author = "NaturesEye", Name = "frequency-of-sleep-meditation", Path = "frequency-of-sleep-meditation-113050.mp3"},
                    new Music { Id = 3, Author = "John Kensy Music", Name = "mindfulness-relaxation-amp-meditation-music", Path = "mindfulness-relaxation-amp-meditation-music-22174.mp3"},
                    new Music { Id = 4, Author = "Alex MakeMusic", Name = "soft-ambient", Path = "soft-ambient-10782"}
                };
            }
            else if(musicGenre.Genre.ToLower() == "brainwaves")
            {
                musicList = new List<Music>()
                {
                    new Music { Id = 0, Author = "fanchisanchez", Name = "cosmic-universe", Path = "cosmic-universe-1058.mp3"},
                    new Music { Id = 1, Author = "Andrewkn", Name = "cosmic-glow", Path = "cosmic-glow-6703.mp3"},
                    new Music { Id = 2, Author = "fanchisanchez", Name = "horizon", Path = "horizon-1056.mp3"}
                };
            }
            else if(musicGenre.Genre.ToLower() == "insomnia")
            {
                musicList = new List<Music>()
                {
                    new Music { Id = 0, Author = "RelaxingTime", Name = "Muzyka Relaksująca", Path = "muzyka-relaksujaca-21783.mp3"},
                    new Music { Id = 1, Author = "MGYKmusic", Name = "mike-gora-elemental", Path = "mike-gora-elemental-108529.mp3"}
                };
            }
            else if(musicGenre.Genre.ToLower() == "classicmusic")
            {
                musicList = new List<Music>()
                {
                    new Music { Id = 0, Author = "LucasKingPiano", Name = "Sociopath", Path = "Dark_Piano_-_Sociopath.mp3"}
                    //new Music { Id = 1, Author = "Thom Yorke", Name = "5.17", Path = "ThomYorke517.mp3"}
                };
            }
            

            CrossMediaManager.Current.Init();

            ElapsedTime = "00:00";
            Duration = "00:00:00";

            SliderValue = 0;

            SliderMax = 1500;
        }



        void Init()
        {
            Duration = "5:30";
            ElapsedTime = "0:00";
        }

        //[ICommand]
        async Task Play(string musicName)
        {
            Debug.WriteLine(musicName);
            Init();

            CrossMediaManager.Current.Dispose();
            CrossMediaManager.Current.Init();
            //await Task.Delay(2000);

            player = await CrossMediaManager.Current.PlayFromAssembly(musicName);
            isPlaying = true;

            await Task.Delay(1000);

            var formattingPattern = @"hh\:mm\:ss";
            var dur = CrossMediaManager.Current.Duration.ToString(formattingPattern);
            var ticks = CrossMediaManager.Current.Duration.Ticks;

            await Device.InvokeOnMainThreadAsync(() =>
            {
                Duration = dur;
                SliderMax = ticks;
            });

            CrossMediaManager.Current.PositionChanged += Current_PositionChanged;
            CrossMediaManager.Current.StateChanged += Current_StateChanged;
            CrossMediaManager.Current.MediaItemFinished += Current_MediaItemFinished;
            CrossMediaManager.Current.MediaItemFailed += Current_MediaItemFailed;

            

        }

        private void Current_MediaItemFailed(object sender, MediaManager.Media.MediaItemFailedEventArgs e)
        {
            throw new NotImplementedException();
        }

        int count = 1;
        private async void Current_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            var listCount = MusicList.Count;

            if (count < listCount)
            {
                await Play(MusicList[count].Path);
                count++;
            }
        }

        private void Current_StateChanged(object sender, MediaManager.Playback.StateChangedEventArgs e)
        {
            if(e.State == MediaManager.Player.MediaPlayerState.Paused)
            {
                isPlaying = false;
                //await CrossMediaManager.Current.Play();
            }
            else if(e.State == MediaManager.Player.MediaPlayerState.Playing)
            {
                isPlaying = true;
                //await CrossMediaManager.Current.Pause();
            }
        }

        private void Current_PositionChanged(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        {

            //sliderValue = CrossMediaManager.Current.Position.Ticks;
            TimeSpan currentMediaPosition = CrossMediaManager.Current.Position;
            TimeSpan currentMediaDuration = CrossMediaManager.Current.Duration;

            var formattingPattern = @"hh\:mm\:ss";
            if (CrossMediaManager.Current.Duration.Hours <= 0)
                formattingPattern = @"mm\:ss";

            var fullLengthString =
                CrossMediaManager.Current.Duration.ToString(formattingPattern);

            ElapsedTime = $"{CrossMediaManager.Current.Position.ToString(formattingPattern)}/{fullLengthString}";

            SliderValue = CrossMediaManager.Current.Position.Ticks;
            //Debug.WriteLine(CrossMediaManager.Current.Duration.Ticks);
        }

        //public static void SliderPositionChanged(double value)
        //{
        //    CrossMediaManager.Current.PositionChanged += Current_PositionChanged1;
        //}

        //private static void Current_PositionChanged1(object sender, MediaManager.Playback.PositionChangedEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}


        [ICommand]
        async Task Play()
        {
            await Application.Current.MainPage.DisplayAlert("warning", "please select the track first", "Ok");

            Debug.WriteLine("fired");
            if (isPlaying)
            {
                await CrossMediaManager.Current.Pause();
                //await Task.Delay(1000);
            }
            else
            {
                await CrossMediaManager.Current.Play();
                //await Task.Delay(1000);
            }
            //await CrossMediaManager.Current.PlayFromResource("assets:///sound.mp3");
            //await CrossMediaManager.Current.Pause();
        }


        [ICommand]
        async Task Forward()
        {
            await CrossMediaManager.Current.StepForward();
        }

        [ICommand]
        async Task Backward()
        {
            await CrossMediaManager.Current.StepBackward();
        }


        //public async Task PlayMusic()
        //{
        //    await CrossMediaManager.Current.PlayFromResource("assets:///sound.mp3");
        //    await CrossMediaManager.Current.Play();
        //}

    }
}
