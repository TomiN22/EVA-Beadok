#nullable enable
using RobotMalacMaui.ViewModel;
using RobotmalacModel.Model;
using RobotmalacModel.Persistence;
using RobotMalacMaui.View;
//using AndroidX.Lifecycle;

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
                await DisplayAlert("Hunt",
                    e.Winner +" won!"
                    ,
                    "OK");
            }

            if (_robotGameModel.IsGameOver)
            {
                if (_robotGameModel.Table.GetSize == 3)
                {
                    
                    //_size = 3;
                    _robotGameModel = new RobotModel(_robotDataAccess, _robotGameModel.Table.GetSize);

                    _robotGameModel = new RobotModel(_robotDataAccess, _robotGameModel.Table.GetSize);
                    _robotViewModel.Model = _robotGameModel;
                    _robotViewModel.CreateTable();
                    _robotGameModel.GameOver += RobotModel_GameOver;
                    //remainingCountLabel.Text = _model.Table.Moves.ToString();
                    //GenerateTable();
                }
                else if (_robotGameModel.Table.GetSize == 5)
                {
                    
                    //_size = 5;
                    _robotGameModel = new RobotModel(_robotDataAccess, _robotGameModel.Table.GetSize);

                    _robotGameModel = new RobotModel(_robotDataAccess, 5);
                    _robotViewModel.Model = _robotGameModel;
                    _robotViewModel.CreateTable();
                    _robotGameModel.GameOver += RobotModel_GameOver;
                    //remainingCountLabel.Text = _model.Table.Moves.ToString();
                    //GenerateTable();
                }
                else if (_robotGameModel.Table.GetSize == 7)
                {
                    
                    //_size = 7;
                    _robotGameModel = new RobotModel(_robotDataAccess, _robotGameModel.Table.GetSize);

                    _robotGameModel = new RobotModel(_robotDataAccess, 5);
                    _robotViewModel.Model = _robotGameModel;
                    _robotViewModel.CreateTable();
                    _robotGameModel.GameOver += RobotModel_GameOver;
                    //remainingCountLabel.Text = _model.Table.Moves.ToString();
                    //GenerateTable();
                }

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
                await DisplayAlert("Hunt", "Load successful.", "OK");

                // csak akkor indul az időzítő, ha sikerült betölteni a játékot
                //StartTimer();
            }
            catch
            {
                await DisplayAlert("Hunt", "Load failed.", "OK");
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
                await DisplayAlert("Hunt", "Save successful.", "OK");
            }
            catch
            {
                await DisplayAlert("Hunt", "Save failed.", "OK");
            }
        }
    }
}
