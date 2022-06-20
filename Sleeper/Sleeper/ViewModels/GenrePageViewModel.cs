using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Sleeper.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Sleeper.Views;

namespace Sleeper.ViewModels
{
    public partial class GenrePageViewModel : ObservableObject
    {

        [ObservableProperty]
        List<MusicGenre> musicGenre;


        MusicGenre previouslySelected;
        MusicGenre selectedGenre;
        public MusicGenre SelectedGenre
        {
            get => selectedGenre;
            set
            {
                if (value != null)
                {
                    //Application.Current.MainPage.DisplayAlert("selected", value.Name, "ok");
                    previouslySelected = value;
                    Application.Current.MainPage.Navigation.PushAsync(new SleeperMainPage(previouslySelected)).GetAwaiter();
                    //await Navigation.PushAsync(new DashboardPage(kids));
                    value = null;
                }

                selectedGenre = value;
                OnPropertyChanged();
            }
        }

        public GenrePageViewModel()
        {
            musicGenre = new List<MusicGenre>()
            {
                new MusicGenre {  Genre= "Chill", Image = "chill.jpg"},
                new MusicGenre {  Genre= "Ambient", Image = "ambient.jpg"},
                new MusicGenre {  Genre= "BrainWaves", Image = "brainwaves2.jpg"},
                new MusicGenre {  Genre= "Insomnia", Image = "Insomnia.jpg"},
                new MusicGenre {  Genre= "ClassicMusic", Image = "classicmusic.jpg"}
            };


        }

    }
}
