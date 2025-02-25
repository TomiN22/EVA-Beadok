
using RobotMalacMaui.ViewModel;
using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
using RobotMalacMaui.View;

namespace RobotMalacMaui
{
    public partial class AppShell : Shell
    {
        private IRobotDataAccess _robotDataAccess;
        private readonly RobotMalacViewModel _robotViewModel;
        private RobotModel _robotGameModel;

        private readonly IStore _store;
        private readonly StoredGameBrowserModel _storedGameBrowserModel;
        private readonly StoredGameBrowserViewModel _storedGameBrowserViewModel;

        public AppShell(IStore robotStore, IRobotDataAccess robotDataAccess, RobotModel robotGameModel, RobotMalacViewModel robotViewModel)
        {
            InitializeComponent();

            // játék összeállítása
            _store = robotStore;
            _robotDataAccess = robotDataAccess;
            _robotGameModel = robotGameModel;
            _robotViewModel = robotViewModel;

            _robotGameModel.GameOver += RobotModel_GameOver;

            _robotViewModel.NewGame += RobotViewModel_NewGame;
            _robotViewModel.HowToGame += RobotViewModel_HowToGame;
            _robotViewModel.LoadGame += RobotViewModel_LoadGame;
            _robotViewModel.SaveGame += RobotViewModel_SaveGame;
            _robotViewModel.ExitGame += RobotViewModel_ExitGame;

            // a játékmentések kezelésének összeállítása
            _storedGameBrowserModel = new StoredGameBrowserModel(_store);
            _storedGameBrowserViewModel = new StoredGameBrowserViewModel(_storedGameBrowserModel);
            _storedGameBrowserViewModel.GameLoading += StoredGameBrowserViewModel_GameLoading;
            _storedGameBrowserViewModel.GameSaving += StoredGameBrowserViewModel_GameSaving;

        }

        private async void RobotModel_GameOver(object? sender, RobotEventArgs e)
        {
            //StopTimer();

            if (e.IsWon)
            {
                // győzelemtől függő üzenet megjelenítése
                await DisplayAlert("Robot-pig fight",
                    e.Winner +" won!"
                    ,
                    "OK");
            }
            
        }

        private void RobotViewModel_NewGame(object? sender, int n)
        {
            _robotGameModel.GameOver -= RobotModel_GameOver;
            _robotViewModel.RemoveTable();
            _robotGameModel = new RobotModel(new IRobotFileDataAccess(String.Empty), n);
            _robotViewModel.Model =_robotGameModel;
            _robotViewModel.CreateTable();
            _robotGameModel.GameOver += RobotModel_GameOver;
        }

        private async void RobotViewModel_LoadGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new LoadGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }

        private async void RobotViewModel_SaveGame(object? sender, EventArgs e)
        {
            await _storedGameBrowserModel.UpdateAsync(); // frissítjük a tárolt játékok listáját
            await Navigation.PushAsync(new SaveGamePage
            {
                BindingContext = _storedGameBrowserViewModel
            }); // átnavigálunk a lapra
        }

        private async void RobotViewModel_HowToGame(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new HowToPage
            {
                BindingContext = _robotViewModel
            }); // átnavigálunk a How to play lapra
        }
        private async void RobotViewModel_ExitGame(object? sender, EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage
            {
                BindingContext = _robotViewModel
            }); // átnavigálunk a beállítások lapra
        }

        /// <summary>
        ///     Betöltés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameLoading(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk

            // betöltjük az elmentett játékot, amennyiben van
            try
            {
                await _robotGameModel.LoadGameAsync(FileSystem.AppDataDirectory + @"/" + e.Name);

                // sikeres betöltés
                await Navigation.PopAsync(); // visszanavigálunk a játék táblára
                _robotViewModel.LoadCreateTable();
                await DisplayAlert("Robot-pig fight", "Load successful.", "OK");

                // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                //StartTimer();
            }
            catch
            {
                await DisplayAlert("Robot-pig fight", "Load failed.", "OK");
            }
        }

        /// <summary>
        ///     Mentés végrehajtásának eseménykezelője.
        /// </summary>
        private async void StoredGameBrowserViewModel_GameSaving(object? sender, StoredGameEventArgs e)
        {
            await Navigation.PopAsync(); // visszanavigálunk
            //StopTimer();

            try
            {
                // elmentjük a játékot
                await _robotGameModel.SaveGameAsync(FileSystem.AppDataDirectory + @"/" + e.Name);
                await DisplayAlert("Robot-pig fight", "Save successful.", "OK");
            }
            catch
            {
                await DisplayAlert("Robot-pig fight", "Save failed.", "OK");
            }
        }
    }
}
