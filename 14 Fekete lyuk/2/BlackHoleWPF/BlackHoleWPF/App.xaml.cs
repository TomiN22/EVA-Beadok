using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using BlackHoleModel.Model;
using BlackHoleModel.Persistence;
using BlackHoleWPF.ViewModel;
using Microsoft.Win32;

namespace BlackHoleWPF
{
    public partial class App : Application
    {
        private GameModel _model = null!;
        private BHViewModel _viewModel = null!;
        private MainWindow _view = null!;
        private IBlackHoleDataAccess? _dataAccess;
        private DispatcherTimer _timer;

        public App()
        {
            _timer = new DispatcherTimer();
            Startup += new StartupEventHandler(App_Startup);
        }

        private void App_Startup(object? sender, StartupEventArgs e)
        {
            // modell létrehozása
            _dataAccess = new BlackHoleFileDataAccess();
            _model = new GameModel(_dataAccess, 0);
            _model.GameOver += new EventHandler<BlackHoleEventArgs>(Model_GameOver);

            // nézemodell létrehozása
            _viewModel = new BHViewModel(_model);
            _viewModel.FiveGame += new EventHandler(ViewModel_Five);
            _viewModel.SevenGame += new EventHandler(ViewModel_Seven);
            _viewModel.NineGame += new EventHandler(ViewModel_Nine);
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
            //_model.AdvanceTime();
        }

        private void ViewModel_Five(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 5);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
        }
        private void ViewModel_Seven(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 7);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
        }

        private void ViewModel_Nine(object? sender, EventArgs e)
        {
            _model = new GameModel(_dataAccess, 9);
            _viewModel.Model = _model;
            _viewModel.CreateTable();
            _model.GameOver += Model_GameOver;
            _timer.Start();
        }

        private void ViewModel_ExitGame(object? sender, System.EventArgs e)
        {
            _view.Close(); // ablak bezárása
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
                openFileDialog.Title = "Loading Black Hole";
                openFileDialog.Filter = "BH table|*.stl";
                if (openFileDialog.ShowDialog() == true)
                {
                    // játék betöltése
                    //await _model.LoadGameAsync(openFileDialog.FileName);
                    _viewModel.Path = openFileDialog.FileName;
                    //_timer.Start();
                }
            }
            catch (BlackHoleDataException)
            {
                MessageBox.Show("File load unsuccessful!", "Black Hole", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            /*if (restartTimer) // ha szükséges, elindítjuk az időzítőt
                _timer.Start();*/
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
                    catch (BlackHoleDataException)
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

        private void Model_GameOver(object? sender, BlackHoleEventArgs e)
        {
            _timer.Stop();
            _viewModel.EnableButtons = false;

            MessageBox.Show(_viewModel.Player+" won!", "Game over!");
        }
    }

}
