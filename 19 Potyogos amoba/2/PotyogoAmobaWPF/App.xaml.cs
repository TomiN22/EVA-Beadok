using Microsoft.Win32;
using PotyogosAmobaWPF.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using PotyogosAmobaModel.Model;
using PotyogosAmobaModel.Persistence;

namespace PotyogoAmobaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _model = null!;
        private PViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private IPotyogosAmobaDataAccess? _dataAccess;
        private DispatcherTimer _timer;
        private int timeRemainingPlayer1 = 300; // 5 perc (300 másodperc)
        private int timeRemainingPlayer2 = 300;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _dataAccess = new PotyogosAmobaFileDataAccess();
            _model = new GameModel(_dataAccess, 0);
            _model.GameOver += new EventHandler<PotyogosAmobaEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new PViewModel(_model);
            _viewModel.TenGame += new EventHandler(ViewModel_TenGame);
            _viewModel.TwentyGame += new EventHandler(ViewModel_TwentyGame);
            _viewModel.ThirtyGame += new EventHandler(ViewModel_ThirtyGame);
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);

            _viewModel.PauseStart += ViewModel_PauseStartGame;

            // nézet létrehozása
            _view = new MainWindow();
            _view.DataContext = _viewModel;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += new EventHandler(Timer_Tick);

            //_view.Closing += new System.ComponentModel.CancelEventHandler(View_Closing); // eseménykezelés a bezáráshoz
            _view.Show();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            _model.AdvanceTime();
        }

        private void ViewModel_TenGame(object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= Model_GameOver;
            }
            _model = new GameModel(_dataAccess, 10);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
        }

        private void ViewModel_TwentyGame(object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= Model_GameOver;
            }
            _model = new GameModel(_dataAccess, 20);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
        }

        private void ViewModel_ThirtyGame(object? sender, EventArgs e)
        {
            if (_model != null)
            {
                _model.GameOver -= Model_GameOver;
            }
            _model = new GameModel(_dataAccess, 30);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close();
        }

        private void ViewModel_PauseStartGame(object? sender, EventArgs e)
        {

            if (_viewModel.StartPause)
            {
                _timer.Start();
            }
            else
            {
                _timer.Stop();
            }
        }

        /// <summary>
        /// Játék betöltésének eseménykezelője.
        /// </summary>
        private void ViewModel_LoadGame(object? sender, System.EventArgs e)
        {
            //Boolean restartTimer = _timer.IsEnabled;

            //_timer.Stop();

            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog(); // dialógusablak
                openFileDialog.Title = "Loading Loose Robot";
                openFileDialog.Filter = "LR table|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    //await _model.LoadGameAsync(openFileDialog.FileName);
                    _viewModel.Path = openFileDialog.FileName;
                    //_timer.Start();
                }
            }
            catch (PotyogosAmobaDataException)
            {
                MessageBox.Show("File load unsuccessful!", "Loose Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*if (restartTimer) // ha szükséges, elindítjuk az időzítőt*/
            _timer.Start();
        }

        /// <summary>
        /// Játék mentésének eseménykezelője.
        /// </summary>
        private async void ViewModel_SaveGame(object? sender, EventArgs e)
        {
            //Boolean restartTimer = _timer.IsEnabled;

            //_timer.Stop();

            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // dialógablak
                saveFileDialog.Title = "Loading Loose Robot";
                saveFileDialog.Filter = "LR table|*.stl";
                if (saveFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        // játéktábla mentése
                        await _model.SaveGameAsync(saveFileDialog.FileName);
                    }
                    catch (PotyogosAmobaDataException)
                    {
                        MessageBox.Show("File save unsuccessful!");
                    }
                }
            }
            catch
            {
                MessageBox.Show("File save unsuccessful!", "Loose Robot", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //if (restartTimer) // ha szükséges, elindítjuk az időzítőt
            //    _timer.Start();
        }
        private void Model_GameOver(object? sender, PotyogosAmobaEventArgs e)
        {
            _timer.Stop();
            _viewModel.EnableButtons = false;

            if (e.Winner == Win.Player1)
                MessageBox.Show(e.Winner+" has won!", "Game over!");
            if (e.Winner == Win.Player2)
                MessageBox.Show(e.Winner+" has won!", "Game over!");
            if (e.Winner == Win.Draw)
                MessageBox.Show("It's a "+e.Winner+"!", "Game over!");

            //_viewModel.SetUp();
        }
    }

}
